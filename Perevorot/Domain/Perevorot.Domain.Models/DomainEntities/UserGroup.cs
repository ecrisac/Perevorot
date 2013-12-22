using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Perevorot.Domain.Models.DomainEntities
{
    public class UserGroup : PerevorotEntity
    {
        public UserGroup(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        //navigation
        public virtual ICollection<AccessRight> AccessRights
        {
            get { return _accessRights; }
        }

        //navigation
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
            _users.Add(user);
        }

        public void RemoveUser(User user)
        {
            _users.Remove(user);
        }

        private readonly ICollection<AccessRight> _accessRights = new Collection<AccessRight>();
        private readonly ICollection<User> _users = new Collection<User>();
    }
}