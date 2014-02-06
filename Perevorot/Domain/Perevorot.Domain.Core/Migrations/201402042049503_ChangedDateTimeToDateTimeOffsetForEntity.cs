namespace Perevorot.Domain.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDateTimeToDateTimeOffsetForEntity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AccessRight", "Created", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.UserGroup", "Created", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.User", "Created", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Customer", "Created", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customer", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.User", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserGroup", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AccessRight", "Created", c => c.DateTime(nullable: false));
        }
    }
}
