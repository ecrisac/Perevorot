using System.Data.Entity;

namespace Perevorot.Domain.IRepositories
{
    public interface IRepository
    {
        DbContext GetSession();
    }
}