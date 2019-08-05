namespace AspitPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_aktiv_elev : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Aktiv", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Aktiv");
        }
    }
}
