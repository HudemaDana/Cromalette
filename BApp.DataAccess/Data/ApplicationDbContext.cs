using BApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BApp.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserColor> UserColors { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }
    }
}
