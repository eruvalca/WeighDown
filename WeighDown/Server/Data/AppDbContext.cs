using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeighDown.Server.Data.Configuration;
using WeighDown.Shared;
using WeighDown.Shared.Models;

namespace WeighDown.Server.Data
{
    public class AppDbContext : IdentityDbContext<WeighDownUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Contestant> Contestants { get; set; }
        public DbSet<WeightLog> WeightLogs { get; set; }
        public DbSet<WeighInDeadline> WeighInDeadlines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Competition>(c =>
            {
                c.Property(c => c.PlayInAmount).HasColumnType("money");
                c.Property(c => c.ThirdPlacePrizeAmount).HasColumnType("money");
                c.Property(c => c.SecondPlacePrizeAmount).HasColumnType("money");
                c.Property(c => c.FirstPlacePrizeAmount).HasColumnType("money");
            });

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
