using Microsoft.AspNet.Identity.EntityFramework;
using StoryFIN3Demo.Models;
using System.Data.Entity;

namespace DemoFIN3.Core.Models
{
    public class DemoFIN3Context : IdentityDbContext<Account>
    {
        public DemoFIN3Context() : base("name=DemoFIN3Context")
        {
            /*Database.SetInitializer(new MigrateDatabaseToLatestVersion<DemoFIN3Context, DemoFIN3.Core.Migrations.Configuration>());*/
        }

        public static DemoFIN3Context Create()
        {
            return new DemoFIN3Context();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Source> Sources { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Story>()
                .HasMany<Account>(s => s.Accounts)
                .WithMany(c => c.Stories)
                .Map(cs =>
                {
                    cs.MapLeftKey("StoryId");
                    cs.MapRightKey("AccountId");
                    cs.ToTable("StoryAccount");
                });
        }
    }
}