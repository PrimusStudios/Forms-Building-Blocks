using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forms.BuildingBlocks.Interfaces.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        ///     Navigates Back one page and passes parameters.
        /// </summary>
        Task GoBackAsync(Dictionary<string, object> parameters, bool animated = true, bool useModal = false);

        /// <summary>
        ///     Navigates Back one page.
        /// </summary>
        Task GoBackAsync(bool animated = true, bool useModal = false);

        /// <summary>
        ///     Navigates to a Uri.
        /// </summary>
        void NavigateToUriAsync(Uri uri);

        /// <summary>
        ///     Navigates to the root page in the navigation stack.
        /// </summary>
        Task NavigateToRootViewAsync(bool animated = true);

        /// <summary>
        ///     Navigates to the root page in the navigation stack and passes parameters.
        /// </summary>
        Task NavigateToRootViewAsync(Dictionary<string, object> parameters, bool animated = true);

        /// <summary>
        ///     Resets the main page of the app. If only passed a basic page view model, then it will just set the main page as a Page in a Navigation Page,
        /// If passed a MasterDetailPageViewModel, the first view model after the MasterDetail will be the Master and the second will be the Detail,
        /// If passed a Tabbed Page, each page will be added to the tabbed page, from left to right.
        /// </summary>
        Task SetMainPageAsync(Dictionary<string, object> parameters, params Type[] viewModels);

        /// <summary>
        ///     Resets the main page of the app. If only passed a basic page view model, then it will just set the main page as a Page in a Navigation Page,
        /// If passed a MasterDetailPageViewModel, the first view model after the MasterDetail will be the Master and the second will be the Detail,
        /// If passed a Tabbed Page, each page will be added to the tabbed page, from left to right.
        /// </summary>
        Task SetMainPageAsync(params Type[] viewModels);

        /// <summary>
        ///     Navigates to page of Type View Model.
        /// </summary>
        Task NavigateToViewAsync(Type viewModel, bool animated = true,
            bool cachePage = false, bool useModal = false);

        /// <summary>
        ///     Navigates to page of Type View Model and passes parameters.
        /// </summary>
        Task NavigateToViewAsync(Type viewModel, Dictionary<string, object> parameters, bool animated = true,
            bool cachePage = false, bool useModal = false);

        /// <summary>
        ///     Navigates to view model.
        /// </summary>
        Task NavigateToAsync<TViewModel>(bool animated = true, bool cachePage = false,
            bool useModal = false)
            where TViewModel : class;

        /// <summary>
        ///     Navigates to view model and passes parameters.
        /// </summary>
        Task NavigateToAsync<TViewModel>(Dictionary<string, object> parameters, bool animated = true,
            bool cachePage = false, bool useModal = false)
            where TViewModel : class;
    }
}