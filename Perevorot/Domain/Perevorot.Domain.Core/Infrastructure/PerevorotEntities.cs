using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.Core.Infrastructure
{
    public class PerevorotEntities : DbContext 
    {
        public PerevorotEntities()
            : base("name=PerevorotEntities")
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<AccessRight> AccessRights { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Member> MembershipData { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure Code First to ignore PluralizingTableName convention
            // If you keep this convention then the generated tables will have pluralized names.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }



    }
}