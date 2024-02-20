using Autofac;
using DevJobs.Application.Features.JobPostAnalyzing.Services;

namespace DevJobs.Api.Request
{
    public class JobPostCountDataRequestHandler
    {
        public int Days { get; set; } = 30;
        private  IJobPostCountService _jobPostChartService;

        public JobPostCountDataRequestHandler()
        {

        }

        public JobPostCountDataRequestHandler(IJobPostCountService jobPostChartService)
        {
            _jobPostChartService = jobPostChartService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _jobPostChartService = scope.Resolve<IJobPostCountService>();
        }

        public object GetJobPostCountByDate(int days)
        {
            return _jobPostChartService.GetJobPostCountByDays(days);
        }
    }
}
