using System;
using Forms.BuildingBlocks.Exceptions;
using Forms.BuildingBlocks.Interfaces.DI;
using Forms.BuildingBlocks.Interfaces.Navigation;
using Forms.BuildingBlocks.Navigation;
using Xamarin.Forms;

namespace Forms.BuildingBlocks.App
{
    public abstract class BuildingBlocksApplication : Application
    {
        public new static BuildingBlocksApplication Current => (BuildingBlocksApplication)Application.Current;
        protected IServiceProvider ServiceProvider;

        protected INavigationService NavigationService;
        protected BuildingBlocksApplication(IPlatformRegistrations platformRegistrations)
        {
            Setup(platformRegistrations);
        }


        void Setup(IPlatformRegistrations platformRegistrations)
        {
            var Container = Register();
            if (Container == null)
                throw new NotSetupException();

            RegisterInternal(Container);
            platformRegistrations?.Register(Container);

            var viewFactory = new PageFactory(Container);

            RegisterViewsForNavigation(viewFactory);
            Container.RegisterInstance(Container);

            ServiceProvider = Container.Create();
            NavigationService = (INavigationService)ServiceProvider.GetService(typeof(INavigationService));

            Initialized();
        }

        void RegisterInternal(IContainer container)
        {
            container.Register<INavigationService, NavigationService>();
            container.Register<IPageNavigation, PageNavigation>();
        }

        protected abstract IContainer Register();
        protected abstract void RegisterViewsForNavigation(IPageFactory pageFactory);
        protected abstract void Initialized();
    }
}