using System;

namespace Perevorot.Domain.Models.Exceptions
{
    public class FailedLoginException : ApplicationException
    {
        public FailedLoginException(string reason):base(reason)
        {
            
        }
    }
}