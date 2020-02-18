using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public NavigationService(IPageFactory pageFactory, IPageNavigation pageNavigation, IServiceProvider serviceProvider)
        {
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
            await GoBackAsync(new Dictionary<string, object>(), useModal, animated);
        }

        public void NavigateToUriAsync(Uri uri)
        {
            Device.OpenUri(uri);
        }

        public async Task NavigateToRootViewAsync(bool animated = true)
        {
            await NavigateToRootViewAsync(new Dictionary<string, object>(), animated);
        }

        public async Task NavigateToRootViewAsync(Dictionary<string, object> parameters, bool animated = true)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            parameters.Add(nameof(NavigationDirection), NavigationDirection.Backward);
            await PageNavigation.Navigation.PopToRootAsync(animated);

            var viewModel = GetCurrentViewModel();
            await HandlePostNavigationInitializations(viewModel, parameters);
        }

        public async Task SetMainPageAsync(Dictionary<string, object> parameters, params Type[] viewModels)
        {
            if (viewModels == null || viewModels.Length == 0)
            {
                throw new ArgumentException("You must pass at least one page view model to set the main page.");
            }

            var firstViewModel = viewModels.First();
            var firstBindingContext = ServiceProvider.GetService(firstViewModel);
            var firstPage = PageFactory.CreatePage(firstViewModel, false);
            firstPage.BindingContext = firstBindingContext;

            switch (firstPage)
            {
                case MasterDetailPage masterDetailPage:
                    await SetMasterDetailPage(parameters, masterDetailPage, viewModels);
                    break;
                case TabbedPage tabbedPage:
                    await SetTabbedPage(parameters, tabbedPage, viewModels);
                    break;
                default:
                    Application.Current.MainPage = new NavigationPage(firstPage);
                    await HandlePostNavigationInitializations(firstBindingContext, parameters);
                    break;
            }

        }

        async Task SetTabbedPage(Dictionary<string, object> parameters, TabbedPage tabbedPage,
            Type[] viewModels)
        {
            if (viewModels.Length < 2)
            {
                throw new Exception("Tabbed page needs at least one page.");
            }

            var initializers = new List<Task>{ HandlePostNavigationInitializations(tabbedPage.BindingContext, parameters) };
            for (var i = 1; i < viewModels.Length; ++i)
            {
                var tabViewModel = viewModels[i];
                var tabBindingContext = ServiceProvider.GetService(tabViewModel);
                var tabPage = PageFactory.CreatePage(tabViewModel, false);
                tabPage.BindingContext = tabBindingContext;

                initializers.Add(HandlePostNavigationInitializations(tabBindingContext, parameters));
                tabbedPage.Children.Add(new NavigationPage(tabPage)
                {
                    Title = tabPage.Title
                });
            }


            Application.Current.MainPage = tabbedPage;

            await Task.WhenAll(initializers);
        }

        async Task SetMasterDetailPage(Dictionary<string, object> parameters, MasterDetailPage masterDetailPage, Type[] viewModels)
        {
            if (viewModels.Length > 3)
            {
                throw new Exception("Master detail page can only have 2 pages.");
            }

            var initializers = new List<Task> { HandlePostNavigationInitializations(masterDetailPage.BindingContext, parameters) };
            if (viewModels.Length > 1)
            {
                var detailViewModel = viewModels[1];
                var detailBindingContext = ServiceProvider.GetService(detailViewModel);
                var detailPage = PageFactory.CreatePage(detailViewModel, false);
                detailPage.BindingContext = detailBindingContext;

                initializers.Add(HandlePostNavigationInitializations(detailBindingContext, parameters));

                masterDetailPage.Detail = new NavigationPage(detailPage);
            }

            if (viewModels.Length > 2)
            {
                var masterViewModel = viewModels[2];
                var masterBindingContext = ServiceProvider.GetService(masterViewModel);
                var masterPage = PageFactory.CreatePage(masterViewModel, false);
                masterPage.BindingContext = masterBindingContext;

                initializers.Add(HandlePostNavigationInitializations(masterBindingContext, parameters));

                masterDetailPage.Master = masterPage;
            }
            Application.Current.MainPage = masterDetailPage;

            await Task.WhenAll(initializers);
        }

        public async Task SetMainPageAsync(params Type[] viewModels)
        {
            await SetMainPageAsync(new Dictionary<string, object>(), viewModels);
        }

        public async Task NavigateToViewAsync(Type viewModel, bool animated = true, bool cachePage = false, bool useModal = false)
        {
            await NavigateToViewAsync(viewModel, new Dictionary<string, object>(), animated, cachePage, useModal);
        }

        public async Task NavigateToViewAsync(Type viewModel, Dictionary<string, object> parameters,
            bool animated = true, bool cachePage = false, bool useModal = false)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var bindingContext = ServiceProvider.GetService(viewModel);
            var page = PageFactory.CreatePage(viewModel, cachePage);

            await Navigate(bindingContext, page, parameters, animated, useModal);
        }

        async Task Navigate(object bindingContext, Page page, Dictionary<string, object> parameters, bool animated, bool useModal)
        {
            await HandlePreNavigationInitializations(bindingContext, parameters);

            page.BindingContext = null;
            page.BindingContext = bindingContext;

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

            await HandlePostNavigationInitializations(bindingContext, parameters);
        }

        public async Task NavigateToAsync<TViewModel>(bool animated = true, bool cachePage = false, bool useModal = false) where TViewModel : class
        {
            await NavigateToAsync<TViewModel>(new Dictionary<string, object>(), animated, cachePage, useModal);
        }

        public async Task NavigateToAsync<TViewModel>(Dictionary<string, object> parameters, bool animated = true, 
            bool cachePage = false, bool useModal = false) where TViewModel : class
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var bindingContext = ServiceProvider.GetService(typeof(TViewModel));
            var page = PageFactory.CreatePage<TViewModel>(cachePage);

            await Navigate(bindingContext, page, parameters, animated, useModal);
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

        object GetCurrentViewModel()
        {
            var page = PageNavigation.Navigation.NavigationStack.LastOrDefault();
            return page?.BindingContext;
        }
    }
}