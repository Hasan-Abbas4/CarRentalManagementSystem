namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCouponandBookingModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "DiscountedPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Bookings", "PricePerDay", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "PricePerDay");
            DropColumn("dbo.Bookings", "DiscountedPrice");
        }
    }
}
