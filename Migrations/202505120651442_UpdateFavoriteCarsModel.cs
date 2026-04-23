namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFavoriteCarsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FavoriteCars", "AddedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FavoriteCars", "AddedDate");
        }
    }
}
