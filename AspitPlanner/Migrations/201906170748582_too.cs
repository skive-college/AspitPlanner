namespace AspitPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class too : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Appointments", "Day");
            DropColumn("dbo.Appointments", "Modules");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "Modules", c => c.String());
            AddColumn("dbo.Appointments", "Day", c => c.String());
        }
    }
}
