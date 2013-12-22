using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.IRepositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void AddNewCustomer(string name);
    }
}