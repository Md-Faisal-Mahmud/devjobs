using DevJobs.Domain.Entities;
using DevJobs.Domain.Repositories;
using DevSkill.Data;
using Microsoft.EntityFrameworkCore;

namespace DevJobs.Infrastructure.Repositories
{
    public class TrackMappingRepository : Repository<TrackMapping, Guid>, ITrackMappingRepository
    {
        public TrackMappingRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }
    }
}