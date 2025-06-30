using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Data
{
    public class LocalDbContext : IdentityDbContext<User>
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Important pour Identity !

            // Configuration de votre ApplicationUser
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Fullname).HasMaxLength(125);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Seed des rôles par défaut
            SeedRoles(modelBuilder);

            // Vos configurations existantes pour BidList, Trade, etc.
            // ...
        }

        public DbSet<BidList> BidLists { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<CurvePoint> CurvePoints { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
    

     private void SeedRoles(ModelBuilder modelBuilder)
        {
            var adminRoleId = "1";
            var userRoleId = "2";
            var managerRoleId = "3";

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                }
            );
        }
    }
}