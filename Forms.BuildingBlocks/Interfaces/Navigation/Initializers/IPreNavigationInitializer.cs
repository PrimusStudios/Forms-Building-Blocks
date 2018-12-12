using System.Collections.Generic;

namespace Forms.BuildingBlocks.Interfaces.Navigation.Initializers
{
    public interface IPreNavigationInitializer
    {
        /// <summary>
        ///     Pre navigation method used for initialization.
        /// </summary>
        void Init(Dictionary<string, object> parameters);
    }
}