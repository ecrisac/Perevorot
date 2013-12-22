
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.IRepositories
{
    public interface ILoginRepository : IRepository
    {
        User GetUserByUserNameAndPassword(string username, string password);
    }
}