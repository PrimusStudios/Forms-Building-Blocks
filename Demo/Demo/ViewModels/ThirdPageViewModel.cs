using System;
using System.Collections.Generic;
using System.Text;
using Forms.BuildingBlocks.ViewModels;

namespace Demo.ViewModels
{
    public class ThirdPageViewModel : BindingBase
    {
        string thirdPageText;
        public string ThirdPageText
        {
            get => thirdPageText;
            set => SetProperty(ref thirdPageText, value);
        }
        public ThirdPageViewModel()
        {
            ThirdPageText = "This is the third page";
        }
    }
}
