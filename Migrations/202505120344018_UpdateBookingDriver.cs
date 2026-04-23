namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBookingDriver : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "DriverId", "dbo.Drivers");
            AddForeignKey("dbo.Bookings", "DriverId", "dbo.Drivers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "DriverId", "dbo.Drivers");
            AddForeignKey("dbo.Bookings", "DriverId", "dbo.Drivers", "Id");
        }
    }
}
