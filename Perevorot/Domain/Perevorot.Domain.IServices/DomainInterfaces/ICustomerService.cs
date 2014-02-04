using System.Collections.Generic;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.IServices.DomainInterfaces
{
    public interface ICustomerService : IService
    {
        void AddNewCustomer(string name);

        IList<Customer> GetCustomers();
    }
}