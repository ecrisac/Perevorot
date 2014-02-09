using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using Perevorot.Domain.IRepositories;
using Perevorot.Domain.Models.DomainEntities;
using Perevorot.Web.ResourceLocator;

namespace Perevorot.Web
{
    public class CustomRoleProvider : RoleProvider
    {
        readonly Lazy<ILoginRepository> _loginRepository = new Lazy<ILoginRepository>(IoC.Resolve<ILoginRepository>);
        private string _applicationName;
        private ILoginRepository LoginRepository { get { return _loginRepository.Value; }}
        //  internal DatabaseConnectionInfo ConnectionInfo { get; set; }
        internal bool InitializeCalled { get; set; }

        // Inherited from RoleProvider ==> Forwarded to previous provider if this provider hasn't been initialized
        public override string ApplicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                if (InitializeCalled)
                {
                    throw new NotSupportedException();
                }
                _applicationName = value;
            }
        }

        private List<User> GetUserFromNames(IEnumerable<string> usernames)
        {
            var userProfiles = new List<User>();
            using (LoginRepository.CreateUnitOfWork())
            {
                foreach (string username in usernames)
                {
                    User user =
                        LoginRepository.GetAll<User>().FirstOrDefault(x => x.UserName == username);
                    if (user == null)
                    {
                        throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "NoUserFound {0}",
                                                                          username));
                    }
                    userProfiles.Add(user);
                }
            }
            return userProfiles;
        }

        private IEnumerable<UserRole> GetRoleFromNames(ICollection<string> roleNames)
        {
            var roles = new List<UserRole>(roleNames.Count);
            using (LoginRepository.CreateUnitOfWork())
            {
                foreach (string role in roleNames)
                {
                    UserRole currentRole = LoginRepository.GetAll<UserRole>()
                                                           .FirstOrDefault(x => role == x.RoleName);
                    if (currentRole == null)
                    {
                        throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                                                                          "NoRoleFound {0}", role));
                    }
                    roles.Add(currentRole);
                }
            }
            return roles;
        }

       
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (LoginRepository.CreateUnitOfWork())
            {
                List<User> users = GetUserFromNames(usernames);
                IEnumerable<UserRole> roles = GetRoleFromNames(roleNames);
                foreach (UserRole userRole in roles)
                {
                    foreach (User userProfile in users)
                    {
                        userRole.AddUser(userProfile);
                    }
                }
            }
        }

        
        public override void CreateRole(string roleName)
        {
            UserRole role = FindRole(roleName);
            if (role != null)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "RoleExists {0}",
                                                                  roleName));
            }
            using (LoginRepository.CreateUnitOfWork())
            {
                LoginRepository.SaveOrUpdate(new UserRole { RoleName = roleName });
            }
        }

      
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            UserRole role = FindRole(roleName);
            if (role == null)
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "RoleDoesNotExists {0}",
                                                                  roleName));
            if (role.Users.Count > 0 && throwOnPopulatedRole)
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "RolePopulated {0}",
                                                                  roleName));
            using (LoginRepository.CreateUnitOfWork())
            {
                LoginRepository.Delete(role);
                return true;
            }
        }

       
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            using (LoginRepository.CreateUnitOfWork())
                return
                    LoginRepository.GetAll<User>()
                                    .Where(x => x.UserName == usernameToMatch)
                                    .Select(x => x.UserName)
                                    .ToArray();
        }

       
        public override string[] GetAllRoles()
        {
            using (LoginRepository.CreateUnitOfWork())
                return LoginRepository.GetAll<UserRole>().Select(x => x.RoleName).ToArray();
        }

       
        public override string[] GetRolesForUser(string username)
        {
            using (LoginRepository.CreateUnitOfWork())
            {
                User user = LoginRepository.GetAll<User>().FirstOrDefault(x => x.UserName == username);
                if (user == null)
                {
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "NoUserFound {0}",
                                                                      username));
                }
                return user.UserRoles.Select(x => x.RoleName).ToArray();
            }
        }

        
        public override string[] GetUsersInRole(string roleName)
        {
            using (LoginRepository.CreateUnitOfWork())
            {
                UserRole role = LoginRepository.GetAll<UserRole>().FirstOrDefault(x => x.RoleName == roleName);
                if (role == null)
                {
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "NoRoleFound {0}",
                                                                      roleName));
                }
                return role.Users.Select(x => x.UserName).ToArray();
            }
        }

       
        public override bool IsUserInRole(string username, string roleName)
        {
            using (LoginRepository.CreateUnitOfWork())
            {
                User user = LoginRepository.GetAll<User>().FirstOrDefault(x => x.UserName == username);
                if (user == null)
                {
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "NoUserFound {0}",
                                                                      roleName));
                }
                if (user.CurrentRole == null)
                    return false;
                return user.CurrentRole.RoleName == roleName;
            }
        }

       
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (string rolename in roleNames)
            {
                if (!RoleExists(rolename))
                {
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "NoRoleFound {0}",
                                                                      rolename));
                }
            }

            foreach (string username in usernames)
            {
                foreach (string rolename in roleNames)
                {
                    if (!IsUserInRole(username, rolename))
                    {
                        throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                                                                          "User {0} NotInRole {1}", username,
                                                                          rolename));
                    }
                }
            }
            using (LoginRepository.CreateUnitOfWork())
            {
                var users = GetUserFromNames(usernames);
                var roles = GetRoleFromNames(roleNames);
                foreach (var userRole in roles)
                {
                    foreach (var userProfile in users)
                    {
                        userRole.RemoveUser(userProfile);
                    }
                }
            }
        }
       
        public override bool RoleExists(string roleName)
        {
            return FindRole(roleName) != null;
        }

        private UserRole FindRole(string roleName)
        {
            using (LoginRepository.CreateUnitOfWork())
                return LoginRepository.GetAll<UserRole>().FirstOrDefault(x => x.RoleName == roleName);
        }
    }
}

