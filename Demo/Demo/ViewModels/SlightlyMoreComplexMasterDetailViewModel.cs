using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Forms.BuildingBlocks.Interfaces.Navigation;
using Xamarin.Forms;

namespace Demo.ViewModels
{
    public class SlightlyMoreComplexMasterDetailViewModel : Forms.BuildingBlocks.ViewModels.BindingBase
    {
        readonly INavigationService NavigationService;

        public ICommand ReplaceCommand => new Command(async () => await ReplaceNavigation());
        public ICommand PushCommand => new Command(async () => await PushNavigation());
        public ICommand ColorPushCommand => new Command(async () => await PushColor());      
        public ICommand ColorReplaceCommand => new Command(async () => await ReplaceColor());

        

        public SlightlyMoreComplexMasterDetailViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        async Task ReplaceColor()
        {
            await NavigationService.ReplaceAsync("ColorPickerPage", new Dictionary<string, object>
            {
                { "Color", "Red" }
            });
        }

        async Task PushColor()
        {
            await NavigationService.NavigateToAsync("ColorPickerPage", new Dictionary<string, object>
            {
                { "Color", "Red" }
            });
        }

        async Task PushNavigation()
        {
            await NavigationService.NavigateToAsync("SecondPage");
        }

        async Task ReplaceNavigation()
        {
            await NavigationService.ReplaceAsync("ThirdPage");
        }
    }
}
