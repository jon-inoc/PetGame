using PetGameBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace PetGameBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PlayerData> PlayerDatas { get; set; }

        // New Models
        public DbSet<Pet> Pets { get; set; }
        public DbSet<CatType> CatTypes { get; set; }
        public DbSet<Yard> Yards { get; set; }
        public DbSet<Toy> Toys { get; set; }
        public DbSet<ToyType> ToyTypes { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<DungeonRun> DungeonRuns { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            builder.Entity<User>()
                .HasOne(u => u.PlayerData)
                .WithOne(p => p.User)
                .HasForeignKey<PlayerData>(p => p.UserId);

            // Optional: Define additional constraints or relationships here if needed
            base.OnModelCreating(builder);
        }
    }
}
