using System;
using Autofac;
using Demo.Containers;
using Demo.ViewModels;
using Demo.Views;
using Demo.Views.MasterDetails;
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
            pageFactory.RegisterPage<DetailMasterDetailPageViewModel, DetailMasterDetailPage>();
            pageFactory.RegisterPage<ComplexMasterDetailPageViewModel, ComplexMasterDetailPage>();
            pageFactory.RegisterPage<MainTabbedPageViewModel, MainTabbedPage>();
        }

        protected override async void Initialized()
        {
            InitializeComponent();
            await NavigationService.SetMainPageAsync(typeof(MainTabbedPageViewModel), typeof(MainPageViewModel), typeof(SecondPageViewModel), typeof(ThirdPageViewModel));
            //await NavigationService.SetMainPageAsync(typeof(SimpleMasterDetailPageViewModel), typeof(MainPageViewModel));
            //await NavigationService.SetMainPageAsync(typeof(ComplexMasterDetailPageViewModel), typeof(MainPageViewModel), typeof(SecondPageViewModel));
            //await NavigationService.SetMainPageAsync(typeof(SimpleMasterDetailPageViewModel));
            //await NavigationService.SetMainPageAsync(typeof(MainPageViewModel));
        }
    }
}
