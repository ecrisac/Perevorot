using System;

namespace Perevorot.Domain.Models.DomainEntities
{
    public abstract class AbstractUser : PerevorotEntity
    {
        public string UserName { get; set; }

        protected AbstractUser(string userName)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");
            UserName = userName;
           
        }

        [Obsolete("EF Ctor only!")]
        public AbstractUser()
        {
            
        }
    }
}