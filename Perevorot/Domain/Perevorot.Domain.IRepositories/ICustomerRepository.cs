using System.Collections.Generic;
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.IRepositories
{
    public interface ICustomerRepository : IRepository
    {
        void AddNewCustomer(string name);
        IList<Customer> GetCustomers();
    }
}