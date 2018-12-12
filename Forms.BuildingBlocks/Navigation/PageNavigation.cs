using Forms.BuildingBlocks.Interfaces.Navigation;
using Xamarin.Forms;

namespace Forms.BuildingBlocks.Navigation
{
    public class PageNavigation : IPageNavigation
    {
        public INavigation Navigation => GetBasePageNavigation();

        INavigation GetBasePageNavigation()
        {
            var mainPage = Application.Current.MainPage;

            switch (mainPage)
            {
                case null:
                    return null;
                case MasterDetailPage page:
                    return page.Detail.Navigation;
                default:
                    return mainPage.Navigation;
            }
        }
    }
}
