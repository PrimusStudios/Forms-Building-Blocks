using System;

namespace Forms.BuildingBlocks.Exceptions
{
    internal class NotSetupException : Exception
    {
        public NotSetupException() : base("You must call BuildingBlocks.Init() and pass it an implementation of IContainer")
        {
            
        }
    }
}