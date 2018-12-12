using System;
using Xamarin.Forms;

namespace Forms.BuildingBlocks.Interfaces.Navigation
{
    public interface IPageFactory
    {
        /// <summary>
        ///     Creates a page registered to TViewModel
        /// </summary>
        Page CreatePage<TViewModel>(bool cachePage);

        /// <summary>
        ///     Creates a page registered to Type view model
        /// </summary>
        Page CreatePage(Type viewModel, bool cachePage);

        /// <summary>
        ///     Registers a page to a view model
        /// </summary>
        void RegisterPage<TViewModel, TPage>();

        /// <summary>
        ///     Registers a page to a view model
        /// </summary>
        void RegisterPage(Type viewModel, Type page);

        /// <summary>
        ///     Clears the page cache.
        /// </summary>
        void ClearCache();
    }
}