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
                    if (tabbedPage.CurrentPage is NavigationPage)
                        return tabbedPage.CurrentPage.Navigation;
                    return tabbedPage.Navigation;
                default:
                    return mainPage.Navigation;
            }
        }
    }
}
