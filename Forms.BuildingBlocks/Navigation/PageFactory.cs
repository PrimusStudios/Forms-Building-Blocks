﻿using System;
using System.Collections.Generic;
using System.Linq;
using Forms.BuildingBlocks.App;
using Forms.BuildingBlocks.Exceptions;
using Forms.BuildingBlocks.Interfaces.DI;
using Forms.BuildingBlocks.Interfaces.Navigation;
using Xamarin.Forms;

namespace Forms.BuildingBlocks.Navigation
{
    internal class PageFactory : IPageFactory
    {
        readonly Dictionary<string, (Type ViewModel, Type Page)> PageRegistrations;
        readonly Dictionary<string, Page> PageCache;
        readonly IContainer Container;
        Type NavigationPageType;

        public PageFactory()
        {
            Container = BuildingBlocksApplication.Container;
            PageRegistrations = new Dictionary<string, (Type ViewModel, Type Page)>();
            PageCache = new Dictionary<string, Page>();
        }

        public Page CreatePage<TPage>(bool cachePage)
        {
            return CreatePage(typeof(TPage).Name, cachePage);
        }

        public Page CreatePage(string page, bool cachePage)
        {
            if (!PageRegistrations.ContainsKey(page))
                throw new NotRegisteredException(nameof(page));

            if (cachePage && PageCache.ContainsKey(page))
            {
                return PageCache[page];
            }

            var pageType = PageRegistrations[page];
            var createdPage = Activator.CreateInstance(pageType.Page) as Page;
            if (pageType.ViewModel != null)
            {
                try
                {
                    var bindingContext = BuildingBlocksApplication.ServiceProvider.GetService(pageType.ViewModel);
                    createdPage.BindingContext = bindingContext;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            if (cachePage)
            {
                PageCache.Add(page, createdPage);
            }

            return createdPage;
        }

        public void RegisterPage<TPage>(string name = "")
        {
            RegisterPage(null, typeof(TPage), name);
        }

        public void RegisterPage<TViewModel, TPage>(string name = "")
        {
           RegisterPage(typeof(TViewModel), typeof(TPage), name);
        }

        public void RegisterPage(Type viewmodel, Type page, string name = "")
        {
            if (string.IsNullOrEmpty(name))
                name = page.Name;

            if(page.IsSubclassOf(typeof(NavigationPage)))
            {
                NavigationPageType = page;
                return;
            }

            if (!PageRegistrations.ContainsKey(name))
            {
                PageRegistrations.Add(name, (viewmodel, page));
                if(viewmodel != null)
                    Container.Register(viewmodel);
            }
            else
            {
                PageRegistrations[name] = (viewmodel, page);
            }
        }

        public void ClearCache()
        {
            PageCache.Clear();
        }

        public object CreateViewModel(string page)
        {
            if (!PageRegistrations.ContainsKey(page) && PageRegistrations.Values.FirstOrDefault(x=>x.Page.GetType().Name == page).Page == null)
                throw new NotRegisteredException(nameof(page));

            if (PageCache.ContainsKey(page))
            {
                return PageCache[page].BindingContext;
            }

            var pageType = PageRegistrations[page];
            if (pageType.ViewModel != null)
            {
                return BuildingBlocksApplication.ServiceProvider.GetService(pageType.ViewModel);
            }
            else if(PageRegistrations.Values.FirstOrDefault(x => x.Page.GetType().Name == page).ViewModel != null)
            {
                return BuildingBlocksApplication.ServiceProvider.GetService(PageRegistrations.Values.FirstOrDefault(x => x.Page.GetType().Name == page).ViewModel);
            }

            return null;
        }

        public object CreateViewModel<TPage>()
        {
            return CreateViewModel(nameof(TPage));
        }

        public NavigationPage GetNavigationPage(Page page)
        {
            if (NavigationPageType != null)
                return Activator.CreateInstance(NavigationPageType, page) as NavigationPage;

            return new NavigationPage(page);
        }
    }
}