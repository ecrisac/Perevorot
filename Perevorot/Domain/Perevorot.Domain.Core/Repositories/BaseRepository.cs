
using System.Data.Entity;
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Domain.IRepositories;

namespace Perevorot.Domain.Core.Repositories
{
    public abstract class Repository : IRepository
    {       
        public DbContext GetSession()
        {
            return new PerevorotEntities();
        }
    }
}