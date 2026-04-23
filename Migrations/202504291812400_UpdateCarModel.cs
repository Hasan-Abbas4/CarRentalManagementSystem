namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCarModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "Make", c => c.String(nullable: false));
            AddColumn("dbo.Cars", "Model", c => c.String(nullable: false));
            AddColumn("dbo.Cars", "Category", c => c.String(nullable: false));
            DropColumn("dbo.Cars", "Name");
            DropColumn("dbo.Cars", "Brand");
            DropColumn("dbo.Cars", "Year");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cars", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Cars", "Brand", c => c.String(nullable: false));
            AddColumn("dbo.Cars", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Cars", "Category");
            DropColumn("dbo.Cars", "Model");
            DropColumn("dbo.Cars", "Make");
        }
    }
}
