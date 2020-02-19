using System;
using Xamarin.Forms;

namespace Forms.BuildingBlocks.Interfaces.Navigation
{
    public interface IPageFactory
    {
        /// <summary>
        ///     Creates a page registered to TViewModel
        /// </summary>
        Page CreatePage<TPage>(bool cachePage);

        /// <summary>
        ///     Creates a page registered to Type view model
        /// </summary>
        Page CreatePage(string page, bool cachePage);

        /// <summary>
        ///     Registers a page to a view model
        /// </summary>
        void RegisterPage<TViewModel, TPage>(string name = "");

        /// <summary>
        /// Registers a page without a view model
        /// </summary>
        void RegisterPage<TPage>(string name = "");

        /// <summary>
        ///     Registers a page to a view model
        /// </summary>
        void RegisterPage(Type viewModel, Type page, string name = "");

        /// <summary>
        ///     Clears the page cache.
        /// </summary>
        void ClearCache();

        /// <summary>
        /// Returns the view model registered to the page
        /// </summary>
        object CreateViewModel(string page);

        /// <summary>
        /// Returns the view model registered to the page
        /// </summary>
        object CreateViewModel<TPage>();

        /// <summary>
        /// Gets a registered navigation page.
        /// </summary>
        NavigationPage GetNavigationPage(Page page);
    }
}