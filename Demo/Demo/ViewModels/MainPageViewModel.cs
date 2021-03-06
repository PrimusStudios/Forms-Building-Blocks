﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Demo.Views;
using Forms.BuildingBlocks.Interfaces.Navigation;
using Xamarin.Forms;
using BindingBase = Forms.BuildingBlocks.ViewModels.BindingBase;

namespace Demo.ViewModels
{
    public class MainPageViewModel : BindingBase
    {
        public ICommand StartColorDemoCommand => new Command(async ()=> await StartColorDemo());
        public ICommand StartModalDemoCommand => new Command(async () => await navigationService.NavigateToAsync("ModalPage1", useModal: true));
        async Task StartColorDemo()
        {
            await navigationService.NavigateToAsync<ColorPickerPage>(new Dictionary<string, object>
            {
                {"Color", "Red" }
            });
        }

        readonly INavigationService navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
    }
}
