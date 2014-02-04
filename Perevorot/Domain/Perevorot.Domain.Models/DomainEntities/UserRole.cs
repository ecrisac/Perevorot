using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Perevorot.Domain.Models.DomainEntities
{
    public class UserRole : PerevorotEntity
    {
        private readonly ICollection<AccessRight> _accessRights = new Collection<AccessRight>();
        private readonly ICollection<User> _users = new Collection<User>();
        public string RoleName { get; set; }

        public virtual ICollection<AccessRight> AccessRights
        {
            get { return _accessRights; }
        }

        public virtual ICollection<User> Users
        {
            get { return _users; }
        }

        public void AddAccessRight(AccessRight accessRight)
        {
            _accessRights.Add(accessRight);
        }

        public void RemoveAccessRight(AccessRight accessRight)
        {
            _accessRights.Remove(accessRight);
        }

        public void AddUser(User user)
        {
            if(_users.Contains(user))
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                                                                                 "User {0} AlreadyInRole {1}",
                                                                                 user.UserName,RoleName));
            _users.Add(user);
        }

        public void RemoveUser(User user)
        {
            _users.Remove(user);
        }
    }
}