namespace AspitPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppointmentUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "RegistrationTypeID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "RegistrationTypeID");
        }
    }
}
