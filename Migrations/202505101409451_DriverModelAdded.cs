namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DriverModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ContactNumber = c.String(),
                        LicenseNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Bookings", "DriverId", c => c.Int());
            CreateIndex("dbo.Bookings", "DriverId");
            AddForeignKey("dbo.Bookings", "DriverId", "dbo.Drivers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "DriverId", "dbo.Drivers");
            DropIndex("dbo.Bookings", new[] { "DriverId" });
            DropColumn("dbo.Bookings", "DriverId");
            DropTable("dbo.Drivers");
        }
    }
}
