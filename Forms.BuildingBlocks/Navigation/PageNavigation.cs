using Forms.BuildingBlocks.Interfaces.Navigation;
using Xamarin.Forms;

namespace Forms.BuildingBlocks.Navigation
{
    internal class PageNavigation : IPageNavigation
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
                case TabbedPage tabbedPage:
                    return tabbedPage.CurrentPage.Navigation;
                default:
                    return mainPage.Navigation;
            }
        }
    }
}
