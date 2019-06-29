namespace AspitPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moduleNoteAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModulNotes",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        StudentID = c.Int(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => new { t.Date, t.StudentID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ModulNotes");
        }
    }
}
