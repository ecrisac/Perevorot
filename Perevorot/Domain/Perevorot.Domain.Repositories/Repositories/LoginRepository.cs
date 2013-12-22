using System.Linq;
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Domain.IRepositories;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.Repositories.Repositories
{
    public class LoginRepository : Repository, ILoginRepository
    {
        public LoginRepository(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory)
        {
        }

        public User GetUserByUserNameAndPassword(string username, string password)
        {
            return GetAll<User>().SingleOrDefault(x => x.UserName == username && x.IsActive && x.Password == password);
        }
    }
}