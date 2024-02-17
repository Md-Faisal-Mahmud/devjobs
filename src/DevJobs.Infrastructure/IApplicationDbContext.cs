using DevJobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevJobs.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<Technology> Technology { get; set; }
        DbSet<TrackMapping> TrackMapping { get; set; }
        DbSet<ExperienceMapping> ExperienceMapping { get; set; }
        DbSet<JobPost> JobPost { get; set; }
        DbSet<DbLog> DbLog { get; set; }
    }
}