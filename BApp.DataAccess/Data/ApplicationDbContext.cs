using BApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BApp.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserColor> UserColors { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Level>().HasData(GenerateLevels());
        }

        private List<Level> GenerateLevels()
        {
            List<Level> levels = new List<Level>();

            // Define a rule for LevelTotalXP based on level index
            for (int i = 1; i <= 15; i++)
            {
                Level level = new Level
                {
                    Id = i,
                    LevelName = i,
                    LevelTotalXP = CalculateTotalXPForLevel(i) // Calculate XP based on level index
                };

                levels.Add(level);
            }

            return levels;
        }

        // Define your rule for calculating XP for each level
        private int CalculateTotalXPForLevel(int levelIndex)
        {
            // Example rule: XP doubles with each level (starting from 100)
            int baseXP = 100;
            return baseXP * (int)Math.Pow(2, levelIndex - 1);
        }

    }
}
