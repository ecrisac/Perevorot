using Perevorot.Domain.Core.Infrastructure;

namespace Perevorot.Domain.IRepositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void AddNewCustomer(string name);
    }
}