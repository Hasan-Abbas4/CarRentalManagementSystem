namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBooking : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "PaymentStatus", c => c.String());
            DropTable("dbo.Payments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookingId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPaid = c.Boolean(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        PaymentMethod = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Bookings", "PaymentStatus", c => c.Int(nullable: false));
        }
    }
}
