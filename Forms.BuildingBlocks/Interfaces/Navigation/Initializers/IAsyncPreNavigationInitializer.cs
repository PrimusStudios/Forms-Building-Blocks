using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forms.BuildingBlocks.Interfaces.Navigation.Initializers
{
    public interface IAsyncPreNavigationInitializer
    {
        /// <summary>
        ///     Async Pre navigation method used for initialization.
        /// </summary>
        Task InitAsync(Dictionary<string, object> parameters);
    }
}