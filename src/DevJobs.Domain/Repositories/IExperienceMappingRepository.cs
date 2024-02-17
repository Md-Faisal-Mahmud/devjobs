using DevJobs.Domain.Entities;
using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Domain.Repositories
{
    public interface IExperienceMappingRepository : IRepository<ExperienceMapping, Guid>
    {
    }
}