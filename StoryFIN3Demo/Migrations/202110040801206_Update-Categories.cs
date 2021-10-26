namespace StoryFIN3Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ImageUrl", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "ImageUrl");
        }
    }
}
