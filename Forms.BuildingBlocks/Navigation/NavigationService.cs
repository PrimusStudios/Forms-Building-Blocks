using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forms.BuildingBlocks.Enums;
using Forms.BuildingBlocks.Interfaces.DI;
using Forms.BuildingBlocks.Interfaces.Navigation;
using Forms.BuildingBlocks.Interfaces.Navigation.Initializers;
using Xamarin.Forms;

namespace Forms.BuildingBlocks.Navigation
{
    public class NavigationService : INavigationService
    { 
        readonly IPageFactory pageFactory;
        readonly IPageNavigation pageNavigation;

        public NavigationService(IPageFactory pageFactory, IPageNavigation pageNavigation)
        {
            this.pageFactory = pageFactory;
            this.pageNavigation = pageNavigation;
        }

        public async Task GoBackAsync(Dictionary<string, object> parameters, bool animated = true,
            bool useModal = false)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            if (useModal)
            {

                parameters.Add(nameof(NavigationDirection), NavigationDirection.ModalBackward);
                await pageNavigation.Navigation.PopModalAsync(animated);
            }
            else
            {
                parameters.Add(nameof(NavigationDirection), NavigationDirection.Backward);
                await pageNavigation.Navigation.PopAsync(animated);
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
            await pageNavigation.Navigation.PopToRootAsync(animated);

            var viewModel = GetCurrentViewModel();
            await HandlePostNavigationInitializations(viewModel, parameters);
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

            var bindingContext = BuildingBlocks.Container.Resolve(viewModel);
            var page = pageFactory.CreatePage(viewModel, cachePage);

            await Navigate(bindingContext, page, parameters, animated, useModal);
        }

        async Task Navigate(object bindingContext, Page page, Dictionary<string, object> parameters, bool animated, bool useModal)
        {
            await HandlePreNavigationInitializations(bindingContext, parameters);

            page.BindingContext = null;
            page.BindingContext = bindingContext;

            if (pageNavigation.Navigation == null)
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
            else if (useModal)
            {
                parameters.Add(nameof(NavigationDirection), NavigationDirection.ModalForward);
                await pageNavigation.Navigation.PushModalAsync(page, animated);
            }
            else
            {
                parameters.Add(nameof(NavigationDirection), NavigationDirection.Forward);
                await pageNavigation.Navigation.PushAsync(page, animated);
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

            var bindingContext = BuildingBlocks.Container.Resolve<TViewModel>();
            var page = pageFactory.CreatePage<TViewModel>(cachePage);

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
            var page = pageNavigation.Navigation.NavigationStack.LastOrDefault();
            return page?.BindingContext;
        }
    }
}