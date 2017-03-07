using System;

namespace Ranger.NetCore.Common
{
    public class ApplicationException : Exception
    {
        public ApplicationException(string message)
            : base(message)
        {
            
        }
    }
}