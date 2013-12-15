using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.IServices.DomainInterfaces
{
    public interface ILoginService : IService
    {
        User GetUserByLoginData(string username, string password);
    }
}