namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReportDamage : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DamageReports");
            AlterColumn("dbo.DamageReports", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.DamageReports", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DamageReports");
            AlterColumn("dbo.DamageReports", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.DamageReports", "BookingId");
        }
    }
}
