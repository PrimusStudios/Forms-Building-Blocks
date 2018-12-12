using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forms.BuildingBlocks.Interfaces.Navigation.Initializers
{
    public interface IAsyncPostNavigationInitializer
    {
        /// <summary>
        ///     Async post navigation method used for initialization.
        /// </summary>
        Task OnNavigatedAsync(Dictionary<string, object> parameters);
    }
}