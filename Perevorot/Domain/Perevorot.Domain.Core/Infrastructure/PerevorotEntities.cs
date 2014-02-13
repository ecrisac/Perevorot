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
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<User>()
                .HasMany(x => x.UserRoles)
                .WithMany(x => x.Users)
            .Map(x =>
            {
                x.ToTable("UsersToUserRoles"); 
                x.MapLeftKey("UserId");
                x.MapRightKey("UserRoleId");
            });
        }
    }
}