using System.Data.Entity;
using Perevorot.Domain.Models.DomainEntities;
using Perevorot.Domain.Models.Enums;

namespace Perevorot.Domain.Core.Infrastructure
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<PerevorotEntities>
    {
        protected override void Seed(PerevorotEntities context)
        {
            var user = new User("HarryPotter@Hogwarts.mag");
            var userRole = new UserRole {RoleName = "Operator"};
           
            var accessRight = new AccessRight("HomePage", AccessRightType.ReadAndWrite);
            userRole.AddAccessRight(accessRight);
            userRole.AddUser(user);

            context.Users.Add(user);
            context.AccessRights.Add(accessRight);
            context.UserRoles.Add(userRole);
            var application = new Application
                {
                    ApplicationName = "Perevorot",
                    Description = "Super Application description"
                };
            context.Applications.Add(application);
            context.SaveChanges();


        }
    }
}