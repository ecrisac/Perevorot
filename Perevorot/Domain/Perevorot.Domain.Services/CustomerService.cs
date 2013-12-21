using System;
using Perevorot.Domain.IRepositories;
using Perevorot.Domain.IServices.DomainInterfaces;

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
            _customerRepository.AddNewCustomer(name);
        }
    }
}