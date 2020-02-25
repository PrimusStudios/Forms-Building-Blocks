using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forms.BuildingBlocks.Interfaces.Navigation.Initializers
{
    public interface IAsyncInitialize
    {
        /// <summary>
        ///     Async method used for initialization.
        /// </summary>
        Task InitializeAsync(Dictionary<string, object> parameters);
    }
}