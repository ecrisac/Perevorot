using System;
using System.ComponentModel.DataAnnotations;

namespace Perevorot.Domain.Models.DomainEntities
{
    public abstract class AbstractUser : PerevorotEntity
    {
        [Key]
        public string UserName { get; set; }

        public string Password { get; set; }

        protected AbstractUser(string userName, string password)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");
            if (password == null)
                throw new ArgumentNullException("password");
            UserName = userName;
            Password = password;
        }

        [Obsolete("EF Ctor only!")]
        public AbstractUser()
        {
            
        }
    }
}