using System.Data.Entity.Migrations;
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Domain.Models.DomainEntities;

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
            context.Users.Add(new User("test", "password"));
        }
    }
}