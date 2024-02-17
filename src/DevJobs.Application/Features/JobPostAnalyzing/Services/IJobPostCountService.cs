using DevJobs.Application.Features.JobPostAnalyzing.DTOs;

namespace DevJobs.Application.Features.JobPostAnalyzing.Services
{
    public interface IJobPostCountService
    {
        IList<JobPostCountDTO> GetJobPostCountByDays(int days);
    }
}
