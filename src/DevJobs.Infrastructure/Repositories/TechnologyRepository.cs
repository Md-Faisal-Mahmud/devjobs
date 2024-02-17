using DevJobs.Domain.Entities;
using DevJobs.Domain.Repositories;
using DevSkill.Data;
using Microsoft.EntityFrameworkCore;

namespace DevJobs.Infrastructure.Repositories
{
    public class TechnologyRepository : Repository<Technology, Guid>, ITechnologyRepository
    {
        public TechnologyRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }
    }
}