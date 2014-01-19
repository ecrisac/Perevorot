using System.Data.Entity.Migrations;
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Domain.Models.DomainEntities;
using Perevorot.Domain.Models.Enums;

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

            var user = new User("HarryPotter@Hogwarts.mag", "Phoenix1");
            var userGroup = new UserGroup("Operators");
            var accessRight = new AccessRight("HomePage", AccessRightType.ReadAndWrite);
            userGroup.AddAccessRight(accessRight);
            userGroup.AddUser(user);

            context.Users.Add(user);
            context.AccessRights.Add(accessRight);
            context.UserGroups.Add(userGroup);
            context.SaveChanges();
        }
    }
}