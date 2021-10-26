using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

public class StoryFIN3DemoContext : DbContext
{
    // You can add custom code to this file. Changes will not be overwritten.
    // 
    // If you want Entity Framework to drop and regenerate your database
    // automatically whenever you change your model schema, please use data migrations.
    // For more information refer to the documentation:
    // http://msdn.microsoft.com/en-us/data/jj591621.aspx

    public StoryFIN3DemoContext() : base("name=StoryFIN3DemoContext")
    {
    }

    public System.Data.Entity.DbSet<DemoFIN3.Core.Models.Story> Stories { get; set; }

    public System.Data.Entity.DbSet<DemoFIN3.Core.Models.Author> Authors { get; set; }

    public System.Data.Entity.DbSet<DemoFIN3.Core.Models.Category> Categories { get; set; }
}
