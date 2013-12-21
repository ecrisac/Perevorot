namespace Perevorot.Domain.IRepositories
{
    public interface ICustomerRepository : IRepository
    {
        void AddNewCustomer(string name);
    }
}