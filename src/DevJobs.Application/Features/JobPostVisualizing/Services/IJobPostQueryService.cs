using DevJobs.Domain.Entities;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;

namespace DevJobs.Application.Features.JobPostVisualizing.Services;

public interface IJobPostQueryService
{
    Task<IPaginate<JobPost>> GetPaginatedJobPostsAsync(SearchRequest request);
    Task<JobPost> GetJobPostAsync(Guid id);
}
