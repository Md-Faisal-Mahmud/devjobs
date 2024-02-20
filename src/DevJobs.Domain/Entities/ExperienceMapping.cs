using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Domain.Entities
{
    public class ExperienceMapping : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? Keyword { get; set; }
        public ExperienceLevel Level { get; set; }
    }
}
