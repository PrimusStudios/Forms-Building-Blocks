using System;

namespace Forms.BuildingBlocks.Exceptions
{
    public class NullMainPageException : Exception
    {
        public NullMainPageException() :base("The Main Page of the app cannot be null.  Call SetMainPageAsync to set the main page of the app.")
        {
            
        }
    }
}
