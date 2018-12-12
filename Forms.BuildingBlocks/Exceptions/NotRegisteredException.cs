using System;

namespace Forms.BuildingBlocks.Exceptions
{
    internal class NotRegisteredException : Exception
    {
        public NotRegisteredException(string viewModelName) : base(viewModelName)
        {
            
        }
    }
}