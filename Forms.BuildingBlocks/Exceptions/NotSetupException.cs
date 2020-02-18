using System;

namespace Forms.BuildingBlocks.Exceptions
{
    internal class NotSetupException : Exception
    {
        public NotSetupException() : base("You must provide an implementation of IContainer")
        {
            
        }
    }
}