namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCouponSupport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coupons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        DiscountPercentage = c.Int(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Bookings", "CouponCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "CouponCode");
            DropTable("dbo.Coupons");
        }
    }
}
