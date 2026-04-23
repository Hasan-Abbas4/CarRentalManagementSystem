namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DriverModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "NeedsDriver", c => c.Boolean(nullable: false));
            AddColumn("dbo.Drivers", "IsAvailable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "IsAvailable");
            DropColumn("dbo.Bookings", "NeedsDriver");
        }
    }
}
