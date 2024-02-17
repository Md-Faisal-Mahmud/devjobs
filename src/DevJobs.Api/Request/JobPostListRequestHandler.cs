using DevJobs.Domain.Entities;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;
using Autofac;
using DevJobs.Application.Features.JobPostVisualizing.Services;

namespace DevJobs.Api.Request
{
    public class JobPostListRequestHandler
    {
        private IJobPostQueryService _jobPostQueryService;

        public JobPostListRequestHandler()
        {
        }

        public JobPostListRequestHandler(IJobPostQueryService jobPostQueryService)
        {
            _jobPostQueryService = jobPostQueryService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _jobPostQueryService = scope.Resolve<IJobPostQueryService>();
        }

        public async Task<IPaginate<JobPost>> GetPaginatedJobPostsAsync(SearchRequest request)
        {
            return await _jobPostQueryService.GetPaginatedJobPostsAsync(request);
        }
    }
}