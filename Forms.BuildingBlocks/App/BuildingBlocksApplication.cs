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
        protected IContainer Container;
        protected INavigationService NavigationService;
        protected BuildingBlocksApplication(IPlatformRegistrations platformRegistrations)
        {
            if(BuildingBlocks.Container == null)
                throw new NotSetupException();
            Container = BuildingBlocks.Container;
            Setup(platformRegistrations);
        }

        void Setup(IPlatformRegistrations platformRegistrations)
        {
            RegisterInternal(Container);
            platformRegistrations?.Register(Container);
            Register(Container);
            var viewFactory = Container.Resolve<IPageFactory>();
            RegisterViewsForNavigation(viewFactory);
            NavigationService = Container.Resolve<INavigationService>();
            Initialized();
        }

        void RegisterInternal(IContainer container)
        {
            container.Register<INavigationService, NavigationService>();
            container.RegisterSingleton<IPageFactory, PageFactory>();
            container.Register<IPageNavigation, PageNavigation>();
            container.RegisterInstance(Container);
        }

        protected abstract void Register(IContainer container);
        protected abstract void RegisterViewsForNavigation(IPageFactory pageFactory);
        protected abstract void Initialized();
    }
}