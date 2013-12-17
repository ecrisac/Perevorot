﻿using System;
using System.Collections.Generic;

namespace Perevorot.Domain.Models.DomainEntities
{
    public class User : AbstractUser
    {
        public bool IsActive { get; set; }

        //navigation
        public ICollection<UserGroup> UserGroups { get; private set; }

        public User(string userName, string password)
            : base(userName, password)
        {
            IsActive = true;
        }

        [Obsolete("EF Ctor only!")]
        public User()
        {
            
        }

    }
}