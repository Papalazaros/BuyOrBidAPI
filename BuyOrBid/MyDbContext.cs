using BuyOrBid.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace BuyOrBid.Models
{
    public class MyDbContext : DbContext
    {
#nullable disable
        public DbSet<AutoPost> AutoPosts { get; set; }
        public DbSet<PostActivity> PostActivities { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<User> Users { get; set; }
#nullable restore

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Make>()
                .HasMany<AutoPost>(x => x.AutoPosts)
                .WithOne(x => x.Make!)
                .HasForeignKey(x => x.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Model>()
                .HasMany<AutoPost>(x => x.AutoPosts)
                .WithOne(x => x.Model!)
                .HasForeignKey(x => x.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
