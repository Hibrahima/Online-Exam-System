namespace OnlineExamSytem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Exams", "MyProperty_ID", c => c.Int());
            CreateIndex("dbo.Exams", "MyProperty_ID");
            AddForeignKey("dbo.Exams", "MyProperty_ID", "dbo.Categories", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exams", "MyProperty_ID", "dbo.Categories");
            DropIndex("dbo.Exams", new[] { "MyProperty_ID" });
            DropColumn("dbo.Exams", "MyProperty_ID");
            DropTable("dbo.Categories");
        }
    }
}
