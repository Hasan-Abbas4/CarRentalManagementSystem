namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDamageAndInsuranceHandling : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Wishlists", "CarId", "dbo.Cars");
            DropForeignKey("dbo.Wishlists", "User_Id", "dbo.Users");
            DropIndex("dbo.Wishlists", new[] { "CarId" });
            DropIndex("dbo.Wishlists", new[] { "User_Id" });
            CreateTable(
                "dbo.DamageReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarId = c.Int(nullable: false),
                        Description = c.String(),
                        ReportDate = c.DateTime(nullable: false),
                        EstimatedCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsResolved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .Index(t => t.CarId);
            
            CreateTable(
                "dbo.InsuranceClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DamageReportId = c.Int(nullable: false),
                        ClaimNumber = c.String(),
                        ClaimDate = c.DateTime(nullable: false),
                        ClaimedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsClaimApproved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DamageReports", t => t.DamageReportId, cascadeDelete: true)
                .Index(t => t.DamageReportId);
            
            DropTable("dbo.Wishlists");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Wishlists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        CarId = c.Int(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.InsuranceClaims", "DamageReportId", "dbo.DamageReports");
            DropForeignKey("dbo.DamageReports", "CarId", "dbo.Cars");
            DropIndex("dbo.InsuranceClaims", new[] { "DamageReportId" });
            DropIndex("dbo.DamageReports", new[] { "CarId" });
            DropTable("dbo.InsuranceClaims");
            DropTable("dbo.DamageReports");
            CreateIndex("dbo.Wishlists", "User_Id");
            CreateIndex("dbo.Wishlists", "CarId");
            AddForeignKey("dbo.Wishlists", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Wishlists", "CarId", "dbo.Cars", "Id", cascadeDelete: true);
        }
    }
}
