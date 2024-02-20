using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Domain.Entities
{
    public class JobAnalysis : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public JobPost Post { get; set; }
        public Guid JobPostID { get; set; }
        public Track JobTrack { get; set; }
        public ExperienceLevel Experience { get; set; }
        public List<JobTechnology> Technologies { get; set; }
    }

    public enum Track
    {
        NotSet,
        DotNet,
        Java,
        PHP,
        Cpp,
        iOS,
        Android,
        Ruby,
        Network,
        Graphic,
        SystemAdmin,
        DBA,
        Python,
        Perl,
        Other,
        SQA,
        UX
    }

    public enum ExperienceLevel
    {
        NotSet,
        Intern,
        EntryLevel,
        MidLevel,
        TechLead,
        CLevel
    }
}
