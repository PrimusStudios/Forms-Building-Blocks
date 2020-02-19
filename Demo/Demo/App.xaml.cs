using System;
using Autofac;
using Demo.Containers;
using Demo.ViewModels;
using Demo.Views;
using Demo.Views.MasterDetails;
using Demo.Views.Tabs;
using Forms.BuildingBlocks.App;
using Forms.BuildingBlocks.Interfaces.DI;
using Forms.BuildingBlocks.Interfaces.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Demo
{
    public partial class App : BuildingBlocksApplication
    {
        //Can add platform registrations here.
        public App() : base(null)
        {
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override Forms.BuildingBlocks.Interfaces.DI.IContainer Register()
        {
            var builder = new ContainerBuilder();

            return new AutofacContainer(builder);
        }

        protected override void RegisterViewsForNavigation(IPageFactory pageFactory)
        {
            pageFactory.RegisterPage<MainPageViewModel, MainPage>();
            pageFactory.RegisterPage<SecondPageViewModel, SecondPage>();
            pageFactory.RegisterPage<ThirdPageViewModel, ThirdPage>();
            pageFactory.RegisterPage<ColorPickerPageViewModel, ColorPickerPage>();
            pageFactory.RegisterPage<SimpleMasterDetailPageViewModel, SimpleMasterDetailPage>();
            pageFactory.RegisterPage<MainTabbedPageViewModel, MainTabbedPage>();
            pageFactory.RegisterPage<WrappedTabbedPage>();
            pageFactory.RegisterPage<SimpleMasterDetailPageMaster>();
            pageFactory.RegisterPage<SimpleMasterDetailPageDetail>();
            pageFactory.RegisterPage<SlightlyMoreComplexMasterDetailViewModel, SlightlyMoreComplexMasterDetailPage>();
            pageFactory.RegisterPage<SlightlyMoreComplexMasterDetailPageDetail>();
            pageFactory.RegisterPage<SlightlyMoreComplexMasterDetailPageMaster>();
        }

        protected override async void Initialized()
        {
            InitializeComponent();

            //This tabbed page has individual pages wrapped in navigation pages, the navigation will push in the tabs and leave the
            //tab bar shown on the screen.
            //await NavigationService.SetMainPageAsync("MainTabbedPage");

            //This tabbed page has no navigation pages.  The service will wrap the page in a navigation page. navigation will
            //push on top of the tabbed page, so the tabs will disapear under the new page.
            //await NavigationService.SetMainPageAsync("WrappedTabbedPage");

            //This just shows a simple master detail page
            //await NavigationService.SetMainPageAsync("SimpleMasterDetailPage");

            //This shows the pushing and replacing in a master detail page.
            await NavigationService.SetMainPageAsync("SlightlyMoreComplexMasterDetailPage");


            //await NavigationService.SetMainPageAsync(typeof(SimpleMasterDetailPageViewModel));
            //await NavigationService.SetMainPageAsync(typeof(MainPageViewModel));
        }
    }
}
