namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCouponModelandBookingModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "CouponCode", c => c.String());
            AlterColumn("dbo.Coupons", "Code", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Coupons", "Code", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Bookings", "CouponCode", c => c.String(maxLength: 50));
        }
    }
}
