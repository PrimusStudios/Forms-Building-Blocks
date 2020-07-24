using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Forms.BuildingBlocks.Interfaces.Navigation;
using Forms.BuildingBlocks.ViewModels;
using Xamarin.Forms;

namespace Demo.ViewModels
{
    public class ModalPageViewModel : Forms.BuildingBlocks.ViewModels.BindingBase
    {
        INavigationService NavigationService;

        public ICommand GoBackCommand => new Command(async () => await GoBack());
        public ICommand Page2Command => new Command(async () => await GoToPage2());
        public ICommand Page3Command => new Command(async () => await GoToPage3());
        public ICommand CloseAllCommand => new Command(async()=> await NavigationService.CloseModalAsync());
        public ModalPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        async Task GoBack()
        {
            await NavigationService.GoBackAsync(useModal: true);
        }

        async Task GoToPage2()
        {
            await NavigationService.NavigateToAsync("ModalPage2", useModal: true);
        }

        async Task GoToPage3()
        {
            await NavigationService.NavigateToAsync("ModalPage3", useModal: true);
        }
    }
}
