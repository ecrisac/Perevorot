using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.IServices.DomainInterfaces
{
    public interface ICustomerService : IService
    {
        void AddNewCustomer(string name);
    }
}