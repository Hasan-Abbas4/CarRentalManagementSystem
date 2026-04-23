namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCouponModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Bookings", "CouponCode", c => c.String(maxLength: 50));
            AlterColumn("dbo.Coupons", "Code", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Coupons", "Code", c => c.String(nullable: false));
            AlterColumn("dbo.Bookings", "CouponCode", c => c.String());
            DropColumn("dbo.Bookings", "TotalPrice");
        }
    }
}
