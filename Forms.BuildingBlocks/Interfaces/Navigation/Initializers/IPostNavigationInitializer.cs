using System.Collections.Generic;

namespace Forms.BuildingBlocks.Interfaces.Navigation.Initializers
{
    public interface IPostNavigationInitializer
    {
        /// <summary>
        ///     Post navigation method used for initialization.
        /// </summary>
        void OnNavigated(Dictionary<string, object> parameters);
    }
}