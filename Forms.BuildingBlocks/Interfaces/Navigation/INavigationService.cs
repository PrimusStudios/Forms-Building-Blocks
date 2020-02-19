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
        ///     Navigates to the root page in the navigation stack.
        /// </summary>
        Task NavigateToRootAsync(bool animated = true);

        /// <summary>
        ///     Navigates to the root page in the navigation stack and passes parameters.
        /// </summary>
        Task NavigateToRootAsync(Dictionary<string, object> parameters, bool animated = true);

        /// <summary>
        ///     Resets the main page of the app. With parameter passing.
        /// </summary>
        Task SetMainPageAsync(string page, Dictionary<string, object> parameters);

        /// <summary>
        ///     Resets the main page of the app. 
        /// </summary>
        Task SetMainPageAsync(string page);

        /// <summary>
        ///     Navigates to page of Type View Model.
        /// </summary>
        Task NavigateToAsync(string viewModel, bool animated = true,
            bool cachePage = false, bool useModal = false);

        /// <summary>
        ///     Navigates to page and passes parameters.
        /// </summary>
        Task NavigateToAsync(string page, Dictionary<string, object> parameters, bool animated = true,
            bool cachePage = false, bool useModal = false);

        /// <summary>
        ///     Navigates to Page.
        /// </summary>
        Task NavigateToAsync<TPage>(bool animated = true, bool cachePage = false,
            bool useModal = false)
            where TPage : class;

        /// <summary>
        ///     Navigates to view page and passes parameters.
        /// </summary>
        Task NavigateToAsync<TPage>(Dictionary<string, object> parameters, bool animated = true,
            bool cachePage = false, bool useModal = false)
            where TPage : class;

        /// <summary>
        ///     Replaces the Detail page of a Master detail page, or replaces the current tab of a page,
        ///     or replaces the main page of the app. passes parameters
        /// </summary>
        Task ReplaceAsync<TPage>(Dictionary<string, object> parameters,
            bool cachePage = false)
            where TPage : class;

        /// <summary>
        ///     Replaces the Detail page of a Master detail page, or replaces the current tab of a page,
        ///     or replaces the main page of the app. passes parameters
        /// </summary>
        Task ReplaceAsync(string page, Dictionary<string, object> parameters,
            bool cachePage = false);

        /// <summary>
        ///     Replaces the Detail page of a Master detail page, or replaces the current tab of a page,
        ///     or replaces the main page of the app
        /// </summary>
        Task ReplaceAsync(string page,
            bool cachePage = false);
    }
}