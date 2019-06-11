namespace AspitPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        FromeDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        Day = c.String(),
                        Modules = c.String(),
                        Info = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Presents",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        StudentID = c.Int(nullable: false),
                        Model1 = c.String(),
                        Model2 = c.String(),
                        Model3 = c.String(),
                        Model4 = c.String(),
                    })
                .PrimaryKey(t => new { t.Date, t.StudentID });
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Team = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 50),
                        CatID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Types");
            DropTable("dbo.Students");
            DropTable("dbo.Presents");
            DropTable("dbo.Categories");
            DropTable("dbo.Appointments");
        }
    }
}
