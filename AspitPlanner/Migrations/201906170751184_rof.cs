namespace AspitPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rof : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "Day", c => c.String());
            AddColumn("dbo.Appointments", "Modules", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "Modules");
            DropColumn("dbo.Appointments", "Day");
        }
    }
}
