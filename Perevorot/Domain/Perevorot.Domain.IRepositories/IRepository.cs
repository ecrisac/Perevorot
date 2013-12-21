using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.IRepositories
{
    public interface IRepository<T>
    {
        void Save(User user);
    }
}