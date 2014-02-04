namespace Perevorot.Domain.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessRight",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ResourseId = c.String(),
                        AccessRightType = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        UserRole_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserRole", t => t.UserRole_Id)
                .Index(t => t.UserRole_Id);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Membership",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Password = c.String(),
                        PasswordFormat = c.Int(nullable: false),
                        PasswordSalt = c.String(),
                        MobilePIN = c.String(),
                        Email = c.String(),
                        LoweredEmail = c.String(),
                        PasswordQuestion = c.String(),
                        PasswordAnswer = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        IsLockedOut = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false),
                        LastPasswordChangedDate = c.DateTime(nullable: false),
                        LastLockoutDate = c.DateTime(nullable: false),
                        FailedPasswordAttemptCount = c.Int(nullable: false),
                        FailedPasswordAttemptWindowStart = c.DateTime(nullable: false),
                        FailedPasswordAnswerAttemptCount = c.Int(nullable: false),
                        FailedPasswordAnswerAttemptWindowStart = c.DateTime(nullable: false),
                        Comment = c.String(),
                        Created = c.DateTime(nullable: false),
                        Application_Id = c.Long(),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.Application_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Application_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ApplicationName = c.String(),
                        LoweredApplicationName = c.String(),
                        Description = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(nullable: false),
                        LoweredUserName = c.String(),
                        MobileAlias = c.String(),
                        IsAnonymous = c.Boolean(nullable: false),
                        LastActivityDate = c.DateTime(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        Created = c.DateTime(nullable: false),
                        Application_Id = c.Long(),
                        UserRole_Id = c.Long(),
                        CurrentRole_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.Application_Id)
                .ForeignKey("dbo.UserRole", t => t.UserRole_Id)
                .ForeignKey("dbo.UserRole", t => t.CurrentRole_Id)
                .Index(t => t.Application_Id)
                .Index(t => t.UserRole_Id)
                .Index(t => t.CurrentRole_Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleName = c.String(),
                        Created = c.DateTime(nullable: false),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Membership", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserRole", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "CurrentRole_Id", "dbo.UserRole");
            DropForeignKey("dbo.Users", "UserRole_Id", "dbo.UserRole");
            DropForeignKey("dbo.AccessRight", "UserRole_Id", "dbo.UserRole");
            DropForeignKey("dbo.Users", "Application_Id", "dbo.Applications");
            DropForeignKey("dbo.Membership", "Application_Id", "dbo.Applications");
            DropIndex("dbo.Membership", new[] { "User_Id" });
            DropIndex("dbo.UserRole", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "CurrentRole_Id" });
            DropIndex("dbo.Users", new[] { "UserRole_Id" });
            DropIndex("dbo.AccessRight", new[] { "UserRole_Id" });
            DropIndex("dbo.Users", new[] { "Application_Id" });
            DropIndex("dbo.Membership", new[] { "Application_Id" });
            DropTable("dbo.UserRole");
            DropTable("dbo.Users");
            DropTable("dbo.Applications");
            DropTable("dbo.Membership");
            DropTable("dbo.Customer");
            DropTable("dbo.AccessRight");
        }
    }
}
