using Forms.BuildingBlocks.ViewModels;

namespace Demo.ViewModels
{
    public class SecondPageViewModel : BindingBase
    {
        string secondPageText;
        public string SecondPageText
        {
            get => secondPageText;
            set => SetProperty(ref secondPageText, value);
        }

        public SecondPageViewModel()
        {
            SecondPageText = "This is the second page.";
        }
    }
}
