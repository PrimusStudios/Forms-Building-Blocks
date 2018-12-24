using System;
using Demo.ViewModels;
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

        protected override void Register(IContainer container)
        {
            //container.Register(typeof(MainPageViewModel));
        }

        protected override void RegisterViewsForNavigation(IPageFactory pageFactory)
        {
            pageFactory.RegisterPage<MainPageViewModel, MainPage>();
        }

        protected override async void Initialized()
        {
            InitializeComponent();
            await NavigationService.NavigateToAsync<MainPageViewModel>();
        }
    }
}
