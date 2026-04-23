namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewModelUpdated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "UserId", "dbo.Users");
            AddForeignKey("dbo.Reviews", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "UserId", "dbo.Users");
            AddForeignKey("dbo.Reviews", "UserId", "dbo.Users", "Id");
        }
    }
}
