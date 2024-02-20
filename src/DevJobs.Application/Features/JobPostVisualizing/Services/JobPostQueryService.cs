using DevJobs.Domain.Entities;
using DevSkill.Extensions.Queryable;
using DevSkill.Extensions.Paginate.Contracts;

namespace DevJobs.Application.Features.JobPostVisualizing.Services;

public class JobPostQueryService : IJobPostQueryService
{
    private readonly IApplicationUnitOfWork _unitOfWork;

    public JobPostQueryService(IApplicationUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<JobPost> GetJobPostAsync(Guid id)
    {
        return await _unitOfWork.JobPostRepository.GetJobPostAsync(id);
    }

    public async Task<IPaginate<JobPost>> GetPaginatedJobPostsAsync(SearchRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var result = await _unitOfWork.JobPostRepository.GetPaginatedJobPostsAsync(request);

        return result;
    }
}