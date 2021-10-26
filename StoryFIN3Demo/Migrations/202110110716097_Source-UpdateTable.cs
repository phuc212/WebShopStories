namespace StoryFIN3Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SourceUpdateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Stories", "SourceId", c => c.Int(nullable: false));
            CreateIndex("dbo.Stories", "SourceId");
            AddForeignKey("dbo.Stories", "SourceId", "dbo.Sources", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stories", "SourceId", "dbo.Sources");
            DropIndex("dbo.Stories", new[] { "SourceId" });
            DropColumn("dbo.Stories", "SourceId");
            DropTable("dbo.Sources");
        }
    }
}
