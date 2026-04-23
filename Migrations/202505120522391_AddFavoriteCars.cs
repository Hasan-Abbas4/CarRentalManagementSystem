namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFavoriteCars : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FavoriteCars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CarId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CarId);
            
            DropColumn("dbo.Cars", "Latitude");
            DropColumn("dbo.Cars", "Longitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cars", "Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.Cars", "Latitude", c => c.Double(nullable: false));
            DropForeignKey("dbo.FavoriteCars", "UserId", "dbo.Users");
            DropForeignKey("dbo.FavoriteCars", "CarId", "dbo.Cars");
            DropIndex("dbo.FavoriteCars", new[] { "CarId" });
            DropIndex("dbo.FavoriteCars", new[] { "UserId" });
            DropTable("dbo.FavoriteCars");
        }
    }
}
