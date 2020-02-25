using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forms.BuildingBlocks.App;
using Forms.BuildingBlocks.Enums;
using Forms.BuildingBlocks.Exceptions;
using Forms.BuildingBlocks.Interfaces.Navigation;
using Forms.BuildingBlocks.Interfaces.Navigation.Initializers;
using Xamarin.Forms;

namespace Forms.BuildingBlocks.Navigation
{
    internal class NavigationService : INavigationService
    { 
        readonly IPageFactory PageFactory;
        readonly IPageNavigation PageNavigation;
        readonly IServiceProvider ServiceProvider;
        public NavigationService(IPageFactory pageFactory, IPageNavigation pageNavigation)
        {
            ServiceProvider = BuildingBlocksApplication.ServiceProvider;
            PageFactory = pageFactory;
            PageNavigation = pageNavigation;
        }

        public async Task GoBackAsync(Dictionary<string, object> parameters, bool animated = true,
            bool useModal = false)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            HandleDeInitializations(GetCurrentViewModel(), parameters);

            if (useModal)
            {
                parameters.Add(nameof(NavigationDirection), NavigationDirection.ModalBackward);
                if(PageNavigation.Navigation.ModalStack.Count() > 0)
                    await PageNavigation.Navigation.PopModalAsync(animated);
            }
            else
            {
                parameters.Add(nameof(NavigationDirection), NavigationDirection.Backward);
                await PageNavigation.Navigation.PopAsync(animated);
            }

            var viewModel = GetCurrentViewModel();
            await HandleInitializations(viewModel, parameters);
        }

        public async Task GoBackAsync(bool animated = true, bool useModal = false)
        {
            await GoBackAsync(new Dictionary<string, object>(), animated, useModal);
        }

        public async Task NavigateToRootAsync(bool animated = true)
        {
            await NavigateToRootAsync(new Dictionary<string, object>(), animated);
        }

        public async Task NavigateToRootAsync(Dictionary<string, object> parameters, bool animated = true)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            HandleDeInitializations(GetCurrentViewModel(), parameters);

            parameters.Add(nameof(NavigationDirection), NavigationDirection.Backward);
            await PageNavigation.Navigation.PopToRootAsync(animated);

            var viewModel = GetCurrentViewModel();
            await HandleInitializations(viewModel, parameters);
        }

        public async Task SetMainPageAsync(string page, Dictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(page))
            {
                throw new ArgumentException("You must pass a page to set the main page.");
            }

            if (PageNavigation.Navigation != null)
            {
                HandleDeInitializations(GetCurrentViewModel(), parameters);
            }

            var createdPage = PageFactory.CreatePage(page, false);

            switch (createdPage)
            {
                case MasterDetailPage masterDetailPage:
                    await SetMasterDetailPage(parameters, masterDetailPage);
                    break;
                case TabbedPage tabbedPage:
                    await SetTabbedPage(parameters, tabbedPage);
                    break;
                default:
                    var navPage = PageFactory.GetNavigationPage(createdPage);
                    Application.Current.MainPage = navPage;
                    await HandleInitializations(createdPage.BindingContext, parameters);
                    break;
            }

        }

        async Task SetTabbedPage(Dictionary<string, object> parameters, TabbedPage tabbedPage)
        {
            bool hasNavigationPage = false;


            var initializers = new List<Task> { HandleInitializations(tabbedPage.BindingContext, parameters) };
            hasNavigationPage = tabbedPage.Children.Any(x => x is NavigationPage);

            if (!hasNavigationPage)
            {
                var navTabbedPage = PageFactory.GetNavigationPage(tabbedPage);
                Application.Current.MainPage = navTabbedPage;
            }
            else
            {
                Application.Current.MainPage = tabbedPage;
            }

            for (var i = 0; i < tabbedPage.Children.Count; ++i)
            {
                var tab = tabbedPage.Children[i];
                if (tab is NavigationPage navTab)
                {
                    tab = navTab.CurrentPage;
                }
                var tabPageViewModel = PageFactory.CreateViewModel(tab.GetType().Name);

                if (tabPageViewModel != null)
                {
                    tab.BindingContext = tabPageViewModel;
                    initializers.Add(HandleInitializations(tabPageViewModel, parameters));                    
                }
            }   

            await Task.WhenAll(initializers);
        }

        async Task SetMasterDetailPage(Dictionary<string, object> parameters, MasterDetailPage masterDetailPage)
        {

            Application.Current.MainPage = masterDetailPage;

            var initializers = new List<Task> { HandleInitializations(masterDetailPage.BindingContext, parameters) };
            if (masterDetailPage.Detail != null)
            {
                if (masterDetailPage.Detail is NavigationPage navPage)
                {
                    var detailPageViewModel = PageFactory.CreateViewModel(navPage.CurrentPage.GetType().Name);
                    if (detailPageViewModel != null)
                    {
                        navPage.CurrentPage.BindingContext = detailPageViewModel;
                        initializers.Add(HandleInitializations(detailPageViewModel, parameters));
                    }
                }
                else
                {
                    var detailPageViewModel = PageFactory.CreateViewModel(masterDetailPage.Detail.GetType().Name);
                    if (detailPageViewModel != null)
                    {
                        masterDetailPage.Detail.BindingContext = detailPageViewModel;
                        initializers.Add(HandleInitializations(detailPageViewModel, parameters));
                    }
                }
            }

            if (masterDetailPage.Master != null)
            {
                var masterPageViewModel = PageFactory.CreateViewModel(masterDetailPage.Master.GetType().Name);
                if (masterPageViewModel != null)
                {
                    masterDetailPage.Master.BindingContext = masterPageViewModel;
                    initializers.Add(HandleInitializations(masterDetailPage, parameters));
                }
            }          

            await Task.WhenAll(initializers);
        }

        public async Task SetMainPageAsync(string page)
        {
            await SetMainPageAsync(page, new Dictionary<string, object>());
        }

        public async Task NavigateToAsync(string page, bool animated = true, bool cachePage = false, bool useModal = false)
        {
            await NavigateToAsync(page, new Dictionary<string, object>(), animated, cachePage, useModal);
        }

        public async Task NavigateToAsync(string page, Dictionary<string, object> parameters,
            bool animated = true, bool cachePage = false, bool useModal = false)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var createdPage = PageFactory.CreatePage(page, cachePage);

            await Navigate(createdPage, parameters, animated, useModal);
        }

        async Task Navigate(Page page, Dictionary<string, object> parameters, bool animated, bool useModal)
        {
            HandleDeInitializations(GetCurrentViewModel(), parameters);

            if (PageNavigation.Navigation == null)
            {
                throw new NullMainPageException();
            }

            if (useModal)
            {
                parameters.Add(nameof(NavigationDirection), NavigationDirection.ModalForward);
                await PageNavigation.Navigation.PushModalAsync(page, animated);
            }
            else
            {
                parameters.Add(nameof(NavigationDirection), NavigationDirection.Forward);
                await PageNavigation.Navigation.PushAsync(page, animated);
            }

            await HandleInitializations(page.BindingContext, parameters);
        }

        public async Task NavigateToAsync<TPage>(bool animated = true, bool cachePage = false, bool useModal = false) where TPage : class
        {
            await NavigateToAsync<TPage>(new Dictionary<string, object>(), animated, cachePage, useModal);
        }

        public async Task NavigateToAsync<TPage>(Dictionary<string, object> parameters, bool animated = true, 
            bool cachePage = false, bool useModal = false) where TPage : class
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var page = PageFactory.CreatePage<TPage>(cachePage);

            await Navigate(page, parameters, animated, useModal);
        }

        async Task HandleInitializations(object viewModel, Dictionary<string, object> parameters)
        {
            (viewModel as IInitialize)?.Initialize(parameters);

            if (viewModel is IAsyncInitialize initializer)
            {
                await initializer.InitializeAsync(parameters);
            }

        }

        void HandleDeInitializations(object viewModel, Dictionary<string, object> parameters)
        {
            (viewModel as IDeInitialize)?.DeInitialize(parameters);
        }

        object GetCurrentViewModel()
        {
            if(PageNavigation.Navigation.ModalStack.Any())
            {
                var modalpage = PageNavigation.Navigation.ModalStack.LastOrDefault();
                return modalpage?.BindingContext;
            }

            var page = PageNavigation.Navigation.NavigationStack.LastOrDefault();
            return page?.BindingContext;
        }

        public Task ReplaceAsync<TPage>(Dictionary<string, object> parameters, bool cachePage = false) where TPage : class
        {
            return ReplaceAsync(typeof(TPage).Name, parameters, cachePage);
        }

        public async Task ReplaceAsync(string page, Dictionary<string, object> parameters, bool cachePage = false)
        {
            HandleDeInitializations(GetCurrentViewModel(), parameters);

            parameters.Add(nameof(NavigationDirection), NavigationDirection.Replace);

            var createdPage = PageFactory.CreatePage(page, cachePage);

            var navPage = PageFactory.GetNavigationPage(createdPage);

            switch (Application.Current.MainPage)
            {
                case MasterDetailPage masterPage:
                    masterPage.Detail = navPage;
                    break;
                case TabbedPage tabbedPage:
                    tabbedPage.CurrentPage = navPage;
                    break;
                default:
                    Application.Current.MainPage = navPage;
                    break;
            }

            await HandleInitializations(createdPage.BindingContext, parameters);
        }

        public Task ReplaceAsync(string page, bool cachePage = false)
        {
            return ReplaceAsync(page, new Dictionary<string, object>(), cachePage);
        }

        public bool CanGoBack()
        {
            return Application.Current.MainPage.Navigation.NavigationStack.Count > 1;
        }
    }
}