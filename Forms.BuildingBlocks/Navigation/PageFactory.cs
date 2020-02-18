using System;
using System.Collections.Generic;
using Forms.BuildingBlocks.App;
using Forms.BuildingBlocks.Exceptions;
using Forms.BuildingBlocks.Interfaces.DI;
using Forms.BuildingBlocks.Interfaces.Navigation;
using Xamarin.Forms;

namespace Forms.BuildingBlocks.Navigation
{
    internal class PageFactory : IPageFactory
    {
        readonly Dictionary<Type, Type> PageRegistrations;
        readonly Dictionary<Type, Page> PageCache;
        readonly IContainer Container;

        public PageFactory()
        {
            Container = BuildingBlocksApplication.Container;
            PageRegistrations = new Dictionary<Type, Type>();
            PageCache = new Dictionary<Type, Page>();
        }

        public Page CreatePage<TViewModel>(bool cachePage)
        {
            return CreatePage(typeof(TViewModel), cachePage);
        }

        public Page CreatePage(Type viewModel, bool cachePage)
        {
            if (!PageRegistrations.ContainsKey(viewModel))
                throw new NotRegisteredException(nameof(viewModel));

            if (cachePage && PageCache.ContainsKey(viewModel))
            {
                return PageCache[viewModel];
            }

            var pageType = PageRegistrations[viewModel];
            var page = Activator.CreateInstance(pageType) as Page;

            if (cachePage)
            {
                PageCache.Add(viewModel, page);
            }

            return page;
        }

        public void RegisterPage<TViewModel, TPage>()
        {
           RegisterPage(typeof(TViewModel), typeof(TPage));
        }

        public void RegisterPage(Type viewmodel, Type page)
        {
            if (!PageRegistrations.ContainsKey(viewmodel))
            {
                PageRegistrations.Add(viewmodel, page);
                Container.Register(viewmodel);
            }
            else
            {
                PageRegistrations[viewmodel] = page;
            }
        }

        public void ClearCache()
        {
            PageCache.Clear();
        }
    }
}