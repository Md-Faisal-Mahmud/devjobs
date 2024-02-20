using DevJobs.Domain.Entities;
using DevJobs.Domain.Repositories;
using DevSkill.Data;
using Microsoft.EntityFrameworkCore;

namespace DevJobs.Infrastructure.Repositories
{
    public class ExperienceMappingRepository : Repository<ExperienceMapping, Guid>, IExperienceMappingRepository
    {
        public ExperienceMappingRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }
    }
}