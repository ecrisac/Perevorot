
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.IRepositories
{
    public interface ILoginRepository : IRepository<User>
    {
        User GetUserByUserNameAndPassword(string username, string password);
    }
}