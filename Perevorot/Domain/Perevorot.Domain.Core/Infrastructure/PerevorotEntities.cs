using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Perevorot.Domain.IRepositories;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.Core.Infrastructure
{
    public class PerevorotEntities : DbContext //, IDbContext
    {
        public PerevorotEntities()
            : base("name=PerevorotEntities")
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<AccessRight> AccessRights { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure Code First to ignore PluralizingTableName convention
            // If you keep this convention then the generated tables will have pluralized names.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }



    }
}