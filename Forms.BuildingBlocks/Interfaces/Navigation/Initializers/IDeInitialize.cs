using System.Collections.Generic;

namespace Forms.BuildingBlocks.Interfaces.Navigation.Initializers
{
    public interface IDeInitialize
    {
        /// <summary>
        ///     method used for de-initialization.
        /// </summary>
        void DeInitialize(Dictionary<string, object> parameters);
    }
}