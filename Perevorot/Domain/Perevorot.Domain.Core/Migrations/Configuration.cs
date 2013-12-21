using System.Data.Entity.Migrations;
using Perevorot.Domain.Core.Infrastructure;

namespace Perevorot.Domain.Core.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<PerevorotEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Perevorot.Domain.Core.Infrastructure.PerevorotEntities";
        }

        protected override void Seed(PerevorotEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}