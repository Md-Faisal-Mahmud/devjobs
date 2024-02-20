using Autofac;
using DevJobs.Application.Features.JobPostVisualizing.Services;
using DevJobs.Domain.Entities;

namespace DevJobs.Api.Request
{
    public class JobPostGetRequestHandler
    {
        private IJobPostQueryService _jobPostQueryService;

        public JobPostGetRequestHandler()
        {
        }

        public JobPostGetRequestHandler(IJobPostQueryService jobPostQueryService)
        {
            _jobPostQueryService = jobPostQueryService;
        }

        public void ResolvedDependency(ILifetimeScope scope)
        {
            _jobPostQueryService = scope.Resolve<IJobPostQueryService>();
        }

        public async Task<JobPost>? GetJobPostAsync(Guid id)
        {
            return await _jobPostQueryService.GetJobPostAsync(id);
        }
    }
}
