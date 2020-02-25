using System;
using System.Collections.Generic;

namespace Forms.BuildingBlocks.Interfaces.Navigation.Initializers
{
    public interface INavigatedFromInitializer
    {
        void OnNavigatedFrom(Dictionary<string, object> parameters);
    }
}
