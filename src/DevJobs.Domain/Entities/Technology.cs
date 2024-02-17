using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Domain.Entities
{
    public class Technology : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Track TechnologyTrack { get; set; }
        public string? Name { get; set; }
    }
}
