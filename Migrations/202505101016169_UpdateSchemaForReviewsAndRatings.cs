namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchemaForReviewsAndRatings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "Car_Id", c => c.Int());
            CreateIndex("dbo.Reviews", "Car_Id");
            AddForeignKey("dbo.Reviews", "Car_Id", "dbo.Cars", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "Car_Id", "dbo.Cars");
            DropIndex("dbo.Reviews", new[] { "Car_Id" });
            DropColumn("dbo.Reviews", "Car_Id");
        }
    }
}
