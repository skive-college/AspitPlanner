namespace AspitPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userrolls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Users", "UserRole", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "UserRole");
            DropTable("dbo.UserRoles");
        }
    }
}
