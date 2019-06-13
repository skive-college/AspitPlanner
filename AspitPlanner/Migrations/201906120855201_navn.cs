namespace AspitPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class navn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Types", "CatID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Types", "CatID", c => c.String());
        }
    }
}
