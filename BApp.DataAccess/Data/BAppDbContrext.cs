using BApp.Domain.Models;
using BApp.Domain.Models.External;
using Microsoft.EntityFrameworkCore;

namespace BApp.DataAccess.Data
{
    public class BAppDbContrext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserColor> UserColors { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }
        public DbSet<ExternalColor> ExternalColors { get; set; }
        public BAppDbContrext(DbContextOptions<BAppDbContrext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }


    }
}
