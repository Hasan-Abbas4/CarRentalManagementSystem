namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedCoupon : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bookings", "PricePerDay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "PricePerDay", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
