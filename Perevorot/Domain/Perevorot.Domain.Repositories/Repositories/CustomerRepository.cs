using System.Linq;
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Domain.IRepositories;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.Repositories.Repositories
{
    public class CustomerRepository : Repository, ICustomerRepository
    {
        public CustomerRepository(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory)
        {
        }

        public void AddNewCustomer(string name)
        {
            var newCustomer = new Customer(name);

            SaveOrUpdate(newCustomer);
        }

        public User GetUserByUserNameAndPassword(string username, string password)
        {
            using (CreateUnitOfWork())
            {
                return GetAll<User>().SingleOrDefault(x => x.UserName == username && x.Password == password);
            }
        }
    }
}