using System.Linq;
using Perevorot.Domain.IRepositories;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.Core.Repositories
{
    public class LoginRepository : Repository, ILoginRepository
    {
        public User GetUserByUserNameAndPassword(string username, string password)
        {
            var result = from u in Session.Users
                         where u.UserName == username 
                             select u;
            return result.FirstOrDefault();
        }

        public void Save(User user)
        {
            Session.Users.Attach(user);
            Session.SaveChanges();
        }
    }
}