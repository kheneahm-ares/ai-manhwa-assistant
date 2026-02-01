using Microsoft.EntityFrameworkCore;
using ReadingProgress.Data.Models;

namespace ReadingProgress.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ReadingList> ReadingLists { get; set; }
        public DbSet<ReadingProgressEvent> ReadingProgressEvents { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // db configurations like indexing go here
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ReadingList>()
                .HasIndex(rl => new { rl.UserId, rl.ManhwaId })
                .IsUnique();


            // i'm going to be querying progress events by user, manhwa, and created date desc a lot
            // to get the latest chapter read
            modelBuilder.Entity<ReadingProgressEvent>()
                .HasIndex(rpe => new { rpe.UserId, rpe.ManhwaId, rpe.EventDate })
                .IsDescending(false, false, true);
        }

    }
}
