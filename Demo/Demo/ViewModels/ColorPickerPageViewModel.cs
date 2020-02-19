using System;
using Forms.BuildingBlocks.Interfaces.Navigation;
using Forms.BuildingBlocks.Interfaces.Navigation.Initializers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Demo.Views;

namespace Demo.ViewModels
{
    public class ColorPickerPageViewModel : Forms.BuildingBlocks.ViewModels.BindingBase, IPreNavigationInitializer
    {
        public ICommand GoToNextPageCommand => new Command(async ()=> await NavigateToNextPage());
        public ICommand EndCommand => new Command(async () => await NavigateToHome());
        List<string> _colors = new List<string>();
        async Task NavigateToNextPage()
        {
            await navigationService.NavigateToAsync<ColorPickerPage>(new Dictionary<string, object>
            {
                {"Color", SelectedColorName}
            });
        }

        async Task NavigateToHome()
        {
            await navigationService.NavigateToRootAsync();
        }

        public List<string> Colors
        {
            get => _colors;
            set => SetProperty(ref _colors, value);
        }

        Color _selectedColor;

        public Color SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }
        string _selectedColorName;

        public string SelectedColorName
        {
            get => _selectedColorName;
            set
            {
                SelectedColor = (Color)new ColorTypeConverter().ConvertFromInvariantString(value);
                SetProperty(ref _selectedColorName, value);
            }
        }

        Color _backgroundColor;
        readonly INavigationService navigationService;
        
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

        private string _backgroundColorName;
        public string BackgroundColorName
        {
            get => _backgroundColorName;
            set
            {
                BackgroundColor = (Color) new ColorTypeConverter().ConvertFromInvariantString(value);
                SetProperty(ref _backgroundColorName, value);
            }
        }

        public ColorPickerPageViewModel(INavigationService navigationService)
        {
            
            this.navigationService = navigationService;
            
            foreach (var field in typeof(Xamarin.Forms.Color).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (!string.IsNullOrEmpty(field.Name))
                    _colors.Add(field.Name);
            }
        }

        public void Init(Dictionary<string, object> parameters)
        {

            if (parameters.ContainsKey("Color"))
            {
                BackgroundColorName = (string) parameters["Color"];
                SelectedColorName = (string)parameters["Color"];
            }
        }
    }
}
