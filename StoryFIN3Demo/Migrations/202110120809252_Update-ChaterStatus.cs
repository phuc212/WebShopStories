namespace StoryFIN3Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateChaterStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chapters", "isReading", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chapters", "isReading");
        }
    }
}
