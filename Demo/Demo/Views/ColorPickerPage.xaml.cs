using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColorPickerPage : ContentPage
	{
		public ColorPickerPage ()
		{
			InitializeComponent ();
		}

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
            PickerButton.Clicked += PickerButton_Clicked;
	    }

        private void PickerButton_Clicked(object sender, EventArgs e)
        {
            ColorPicker.Focus();
        }

        protected override void OnDisappearing()
	    {
	        base.OnDisappearing();
	        PickerButton.Clicked -= PickerButton_Clicked;

	    }
	}
}