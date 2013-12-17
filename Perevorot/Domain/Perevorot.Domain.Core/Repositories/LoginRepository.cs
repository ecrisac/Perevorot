
using System.Linq;
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Domain.IRepositories;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.Core.Repositories
{
    public class LoginRepository : Repository, ILoginRepository
    {
        public User GetUserByUserNameAndPassword(string username, string password)
        {
            var perevorotContext = GetSession() as PerevorotEntities;
            var result = from u in perevorotContext.Users
                         where u.UserName == username 
                             select u;

            return result.FirstOrDefault();
        }
    }
}