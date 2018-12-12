using Xamarin.Forms;

namespace Forms.BuildingBlocks.Interfaces.Navigation
{
    public interface IPageNavigation
    {
        /// <summary>
        ///     App Main Page navigation.
        /// </summary>
        INavigation Navigation { get; }
    }
}