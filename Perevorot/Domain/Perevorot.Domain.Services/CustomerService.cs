using System.Collections.Generic;
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Domain.IRepositories;
using Perevorot.Domain.IServices.DomainInterfaces;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;


        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void AddNewCustomer(string name)
        {
            using (IUnitOfWork uow = _customerRepository.CreateUnitOfWork())
            {
                _customerRepository.AddNewCustomer(name);
                uow.Commit();
            }
        }

        public IList<Customer> GetCustomers()
        {
            return _customerRepository.GetCustomers();
        }
    }
}