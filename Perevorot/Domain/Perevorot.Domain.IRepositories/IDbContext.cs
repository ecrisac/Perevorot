using System.Data.Entity;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.IRepositories
{
    public interface IDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<UserGroup> UserGroups { get; set; }
        DbSet<AccessRight> AccessRights { get; set; }
    }
}