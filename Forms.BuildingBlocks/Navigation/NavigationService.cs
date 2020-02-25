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
            await HandlePostNavigationInitializations(viewModel, parameters);
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

            parameters.Add(nameof(NavigationDirection), NavigationDirection.Backward);
            await PageNavigation.Navigation.PopToRootAsync(animated);

            var viewModel = GetCurrentViewModel();
            await HandlePostNavigationInitializations(viewModel, parameters);
        }

        public async Task SetMainPageAsync(string page, Dictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(page))
            {
                throw new ArgumentException("You must pass a page to set the main page.");
            }

            if (PageNavigation.Navigation != null)
            {
                HandleNavigatedFromInitilaizers(GetCurrentViewModel(), parameters);
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
                    await HandlePreNavigationInitializations(createdPage.BindingContext, parameters);
                    Application.Current.MainPage = navPage;
                    await HandlePostNavigationInitializations(createdPage.BindingContext, parameters);
                    break;
            }

        }

        async Task SetTabbedPage(Dictionary<string, object> parameters, TabbedPage tabbedPage)
        {
            bool hasNavigationPage = false;

            var preinitializers = new List<Task> { HandlePreNavigationInitializations(tabbedPage.BindingContext, parameters) };
            var initializers = new List<Task>{ HandlePostNavigationInitializations(tabbedPage.BindingContext, parameters) };
            for (var i = 0; i < tabbedPage.Children.Count; ++i)
            {
                var tab = tabbedPage.Children[i];
                if (tab is NavigationPage navTab)
                {
                    hasNavigationPage = true;
                    tab = navTab.CurrentPage;
                }
                var tabPageViewModel = PageFactory.CreateViewModel(tab.GetType().Name);

                if (tabPageViewModel != null)
                {
                    preinitializers.Add(HandlePreNavigationInitializations(tabPageViewModel, parameters));
                    initializers.Add(HandlePostNavigationInitializations(tabPageViewModel, parameters));
                    tab.BindingContext = tabPageViewModel;
                }
            }

            await Task.WhenAll(preinitializers);

            if (!hasNavigationPage)
            {
                var navTabbedPage = PageFactory.GetNavigationPage(tabbedPage);
                Application.Current.MainPage = navTabbedPage;
            }
            else
            {
                Application.Current.MainPage = tabbedPage;
            }


            await Task.WhenAll(initializers);
        }

        async Task SetMasterDetailPage(Dictionary<string, object> parameters, MasterDetailPage masterDetailPage)
        {
            var initializers = new List<Task> { HandlePostNavigationInitializations(masterDetailPage.BindingContext, parameters) };
            var preinitializers = new List<Task> { HandlePreNavigationInitializations(masterDetailPage.BindingContext, parameters) };
            if (masterDetailPage.Detail != null)
            {
                if (masterDetailPage.Detail is NavigationPage navPage)
                {
                    var detailPageViewModel = PageFactory.CreateViewModel(navPage.CurrentPage.GetType().Name);
                    if (detailPageViewModel != null)
                    {
                        preinitializers.Add(HandlePreNavigationInitializations(detailPageViewModel, parameters));
                        initializers.Add(HandlePostNavigationInitializations(detailPageViewModel, parameters));
                        navPage.CurrentPage.BindingContext = detailPageViewModel;
                    }
                }
                else
                {
                    var detailPageViewModel = PageFactory.CreateViewModel(masterDetailPage.Detail.GetType().Name);
                    if (detailPageViewModel != null)
                    {
                        preinitializers.Add(HandlePreNavigationInitializations(detailPageViewModel, parameters));
                        initializers.Add(HandlePostNavigationInitializations(detailPageViewModel, parameters));
                        masterDetailPage.Detail.BindingContext = detailPageViewModel;
                    }
                }
            }

            if (masterDetailPage.Master != null)
            {
                var masterPageViewModel = PageFactory.CreateViewModel(masterDetailPage.Master.GetType().Name);
                if (masterPageViewModel != null)
                {

                    preinitializers.Add(new Task(async()=> await HandlePreNavigationInitializations(masterPageViewModel, parameters)));
                    initializers.Add(HandlePostNavigationInitializations(masterPageViewModel, parameters));
                    masterDetailPage.Master.BindingContext = masterPageViewModel;
                }
            }

            await Task.WhenAll(preinitializers);

            Application.Current.MainPage = masterDetailPage;

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
            HandleNavigatedFromInitilaizers(GetCurrentViewModel(), parameters);

            await HandlePreNavigationInitializations(page.BindingContext, parameters);

            //page.BindingContext = null;
            //page.BindingContext = bindingContext;

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

            await HandlePostNavigationInitializations(page.BindingContext, parameters);
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

        async Task HandlePostNavigationInitializations(object viewModel, Dictionary<string, object> parameters)
        {
            (viewModel as IPostNavigationInitializer)?.OnNavigated(parameters);

            if (viewModel is IAsyncPostNavigationInitializer initializer)
            {
                await initializer.OnNavigatedAsync(parameters);
            }

        }

        async Task HandlePreNavigationInitializations(object viewModel, Dictionary<string, object> parameters)
        {
            (viewModel as IPreNavigationInitializer)?.Init(parameters);

            if (viewModel is IAsyncPreNavigationInitializer initializer)
            {
                await initializer.InitAsync(parameters);
            }
        }

        void HandleNavigatedFromInitilaizers(object viewModel, Dictionary<string, object> parameters)
        {
            (viewModel as INavigatedFromInitializer)?.OnNavigatedFrom(parameters);
        }

        object GetCurrentViewModel()
        {
            var page = PageNavigation.Navigation.NavigationStack.LastOrDefault();
            return page?.BindingContext;
        }

        public Task ReplaceAsync<TPage>(Dictionary<string, object> parameters, bool cachePage = false) where TPage : class
        {
            return ReplaceAsync(typeof(TPage).Name, parameters, cachePage);
        }

        public async Task ReplaceAsync(string page, Dictionary<string, object> parameters, bool cachePage = false)
        {
            HandleNavigatedFromInitilaizers(GetCurrentViewModel(), parameters);

            parameters.Add(nameof(NavigationDirection), NavigationDirection.Replace);

            var createdPage = PageFactory.CreatePage(page, cachePage);

            await HandlePreNavigationInitializations(createdPage.BindingContext, parameters);

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

            await HandlePostNavigationInitializations(createdPage.BindingContext, parameters);
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