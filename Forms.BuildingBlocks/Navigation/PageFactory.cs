using System;
using System.Collections.Generic;
using Forms.BuildingBlocks.Exceptions;
using Forms.BuildingBlocks.Interfaces.DI;
using Forms.BuildingBlocks.Interfaces.Navigation;
using Xamarin.Forms;

namespace Forms.BuildingBlocks.Navigation
{
    public class PageFactory : IPageFactory
    {
        readonly Dictionary<Type, Type> pageRegistrations;
        readonly Dictionary<Type, Page> pageCache;
        readonly IContainer container;
        public PageFactory()
        {
            pageRegistrations = new Dictionary<Type, Type>();
            pageCache = new Dictionary<Type, Page>();
        }

        public Page CreatePage<TViewModel>(bool cachePage)
        {
            return CreatePage(typeof(TViewModel), cachePage);
        }

        public Page CreatePage(Type viewModel, bool cachePage)
        {
            if (!pageRegistrations.ContainsKey(viewModel))
                throw new NotRegisteredException(nameof(viewModel));

            if (cachePage && pageCache.ContainsKey(viewModel))
            {
                return pageCache[viewModel];
            }

            var pageType = pageRegistrations[viewModel];
            var page = Activator.CreateInstance(pageType) as Page;

            if (cachePage)
            {
                pageCache.Add(viewModel, page);
            }

            return page;
        }

        public void RegisterPage<TViewModel, TPage>()
        {
           RegisterPage(typeof(TViewModel), typeof(TPage));
        }

        public void RegisterPage(Type viewmodel, Type page)
        {
            if (!pageRegistrations.ContainsKey(viewmodel))
            {
                pageRegistrations.Add(viewmodel, page);
                BuildingBlocks.Container.Register(viewmodel);
            }
            else
            {
                pageRegistrations[viewmodel] = page;
            }
        }

        public void ClearCache()
        {
            pageCache.Clear();
        }
    }
}