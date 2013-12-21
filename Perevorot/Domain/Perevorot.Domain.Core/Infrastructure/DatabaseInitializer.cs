using System.Data.Entity;
using Perevorot.Domain.Models.DomainEntities;
using Perevorot.Domain.Models.Enums;

namespace Perevorot.Domain.Core.Infrastructure
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<PerevorotEntities>
    {
        protected override void Seed(PerevorotEntities context)
        {
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