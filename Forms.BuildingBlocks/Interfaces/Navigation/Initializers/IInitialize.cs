using System.Collections.Generic;

namespace Forms.BuildingBlocks.Interfaces.Navigation.Initializers
{
    public interface IInitialize
    {
        /// <summary>
        ///     method used for initialization.
        /// </summary>
        void Initialize(Dictionary<string, object> parameters);
    }
}