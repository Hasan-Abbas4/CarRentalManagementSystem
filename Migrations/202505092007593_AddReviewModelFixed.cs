namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReviewModelFixed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        BookingId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(maxLength: 500),
                        DatePosted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bookings", t => t.BookingId)
                .ForeignKey("dbo.Cars", t => t.CarId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.CarId)
                .Index(t => t.UserId)
                .Index(t => t.BookingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reviews", "CarId", "dbo.Cars");
            DropForeignKey("dbo.Reviews", "BookingId", "dbo.Bookings");
            DropIndex("dbo.Reviews", new[] { "BookingId" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.Reviews", new[] { "CarId" });
            DropTable("dbo.Reviews");
        }
    }
}
