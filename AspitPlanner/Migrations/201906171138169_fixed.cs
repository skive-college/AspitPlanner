namespace AspitPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _fixed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Presents", "Model1", c => c.Int(nullable: false));
            AlterColumn("dbo.Presents", "Model2", c => c.Int(nullable: false));
            AlterColumn("dbo.Presents", "Model3", c => c.Int(nullable: false));
            AlterColumn("dbo.Presents", "Model4", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Presents", "Model4", c => c.String());
            AlterColumn("dbo.Presents", "Model3", c => c.String());
            AlterColumn("dbo.Presents", "Model2", c => c.String());
            AlterColumn("dbo.Presents", "Model1", c => c.String());
        }
    }
}
