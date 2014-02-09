using System;
using System.Data.Entity.Migrations;
using System.Data.SqlTypes;
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
            var application = new Application
            {
                ApplicationName = "Perevorot",
                Description = "Super Application description"
            };
            const string userName = "Harry";
            var userHarry = new User(userName)
            {
                LoweredUserName = userName.ToLower(),
                LastActivityDate = DateTime.Now,
                Application = application
            };

            const string userHarryPotterName = "HarryPotter@Hogwarts.mag";
            var userHarryPotter = new User(userHarryPotterName)
            {
                LoweredUserName = userHarryPotterName.ToLower(),
                LastActivityDate = DateTime.Now,
                Application = application
            };



            var userRole = new UserRole { RoleName = "Operators" };
            var accessRight = new AccessRight("HomePage", AccessRightType.ReadAndWrite);
            userRole.AddAccessRight(accessRight);
            userRole.AddUser(userHarry);

            context.Users.Add(userHarry);

            userRole.AddUser(userHarryPotter);

            context.Users.Add(userHarryPotter);
            context.AccessRights.Add(accessRight);
            context.UserRoles.Add(userRole);
           
            var member = new Member
            {
                Password = "V/mmUWGyAQKWv4G/R8z7sB4q/Ww=",
                Email = "test@gmail.com",
                PasswordQuestion = "",
                PasswordAnswer = "",
                PasswordSalt = "",
                IsApproved = true,
                Comment = string.Empty,
                CreateDate = DateTime.Now,
                LastPasswordChangedDate = DateTime.Now,
                IsLockedOut = false,
                FailedPasswordAttemptCount = 0,
                FailedPasswordAttemptWindowStart = DateTime.Now,
                FailedPasswordAnswerAttemptCount = 0,
                FailedPasswordAnswerAttemptWindowStart = DateTime.Now,
                Application = application,
                LastLockoutDate = SqlDateTime.MinValue.Value,
                LastLoginDate = SqlDateTime.MinValue.Value,
                User = userHarry
            };

            var memberHarryPotter = new Member
            {
                Password = "0VwFfBoSQ3TRwgesiUwKsAlh4b8=",
                Email = "test@gmail.com",
                PasswordQuestion = "",
                PasswordAnswer = "",
                PasswordSalt = "",
                IsApproved = true,
                Comment = string.Empty,
                CreateDate = DateTime.Now,
                LastPasswordChangedDate = DateTime.Now,
                IsLockedOut = false,
                FailedPasswordAttemptCount = 0,
                FailedPasswordAttemptWindowStart = DateTime.Now,
                FailedPasswordAnswerAttemptCount = 0,
                FailedPasswordAnswerAttemptWindowStart = DateTime.Now,
                Application = application,
                LastLockoutDate = SqlDateTime.MinValue.Value,
                LastLoginDate = SqlDateTime.MinValue.Value,
                User = userHarryPotter
            };

            context.MembershipData.Add(member);
            context.MembershipData.Add(memberHarryPotter);
            context.Applications.Add(application);
            context.SaveChanges();
        }
    }
}