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
                        UserGroup_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserGroup", t => t.UserGroup_Id)
                .Index(t => t.UserGroup_Id);
            
            CreateTable(
                "dbo.UserGroup",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(nullable: false),
                        Password = c.String(),
                        Id = c.Long(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserName);
            
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
                "dbo.UserUserGroup",
                c => new
                    {
                        User_UserName = c.String(nullable: false, maxLength: 128),
                        UserGroup_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserName, t.UserGroup_Id })
                .ForeignKey("dbo.User", t => t.User_UserName, cascadeDelete: true)
                .ForeignKey("dbo.UserGroup", t => t.UserGroup_Id, cascadeDelete: true)
                .Index(t => t.User_UserName)
                .Index(t => t.UserGroup_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserUserGroup", "UserGroup_Id", "dbo.UserGroup");
            DropForeignKey("dbo.UserUserGroup", "User_UserName", "dbo.User");
            DropForeignKey("dbo.AccessRight", "UserGroup_Id", "dbo.UserGroup");
            DropIndex("dbo.UserUserGroup", new[] { "UserGroup_Id" });
            DropIndex("dbo.UserUserGroup", new[] { "User_UserName" });
            DropIndex("dbo.AccessRight", new[] { "UserGroup_Id" });
            DropTable("dbo.UserUserGroup");
            DropTable("dbo.Customer");
            DropTable("dbo.User");
            DropTable("dbo.UserGroup");
            DropTable("dbo.AccessRight");
        }
    }
}
