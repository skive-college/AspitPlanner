namespace AspitPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixed_name : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Types", newName: "RegistrationTypes");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.RegistrationTypes", newName: "Types");
        }
    }
}
