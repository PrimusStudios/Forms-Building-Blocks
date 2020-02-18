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
        public static IServiceProvider ServiceProvider { get; private set; }
        internal static IContainer Container { get; private set; }

        protected INavigationService NavigationService;
        protected BuildingBlocksApplication(IPlatformRegistrations platformRegistrations)
        {
            Setup(platformRegistrations);
        }


        void Setup(IPlatformRegistrations platformRegistrations)
        {
            Container = Register();
            if (Container == null)
                throw new NotSetupException();

            RegisterInternal(Container);
            platformRegistrations?.Register(Container);
            var viewFactory = new PageFactory();

            RegisterViewsForNavigation(viewFactory);
            
            Container.RegisterInstance(typeof(IContainer), Container);
            Container.RegisterInstance(typeof(IPageFactory), viewFactory);

            ServiceProvider = Container.Create();
            var test = ServiceProvider.GetService(typeof(IPageFactory));
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