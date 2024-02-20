using DevSkill.Data;

namespace DevJobs.Domain.Entities
{
    public class JobPost : IEntity<Guid>
    {
        public Guid Id {  get; set; }
        public Guid OrganizationID { get; set; }
        public Organization Company { get; set; }
        public string? JobTitle { get; set; }
        public int NumberOfVacancies { get; set; }
        public DateTime PublishedOn { get; set; }
        public string? JobNature { get; set; }
        public int ExperienceMin { get; set; }
        public int? ExperienceMax { get; set; }
        public GenderOption? Gender { get; set; }
        public int? AgeMin { get; set; }
        public int? AgeMax { get; set; }
        public string? JobLocation { get; set; }
        public int? SalaryMin { get; set; }
        public int? SalaryMax { get; set; }
        public string? JobDescription { get; set; }
        public string? EducationalRequirements { get; set; }
        public string? ExperienceRequirements { get; set; }
        public string? AdditionalJobRequirements { get; set; }
        public string? OtherBenefits { get; set; }
        public string? Source { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<JobAnalysis> Analysis { get; set; }
    }

    public enum GenderOption
    {
        Male,
        Female
    }
}
