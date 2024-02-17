using DevJobs.Domain.Entities;
using DevJobs.Infrastructure.Features.Membership;
using DevJobs.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DevJobs.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext(string connectionString, string migrationAssembly) : IdentityDbContext<ApplicationUser,
        ApplicationRole,
        Guid,
        ApplicationUserClaim,
        ApplicationUserRole,
        ApplicationUserLogin,
        ApplicationRoleClaim,
        ApplicationUserToken>,
        IApplicationDbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString,
                    x => x.MigrationsAssembly(migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasData(ApplicationUserSeed.Users);

            modelBuilder.Entity<ApplicationRole>().HasData(ApplicationRolesSeed.Roles);

            modelBuilder.Entity<ApplicationUserRole>().HasData(ApplicationUserRolesSeed.UserRoles);

            modelBuilder.Entity<ApplicationUserClaim>().HasData(ApplicationUserClaimSeed.Claims);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Technology> Technology { get; set; }
        public DbSet<TrackMapping> TrackMapping { get; set; }
        public DbSet<ExperienceMapping> ExperienceMapping { get; set; }
        public DbSet<JobPost> JobPost { get; set; }
        public DbSet<DbLog> DbLog { get; set; }
    }
}