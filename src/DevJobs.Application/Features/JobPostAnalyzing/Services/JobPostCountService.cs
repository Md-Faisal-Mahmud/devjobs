using DevJobs.Application.Features.JobPostAnalyzing.DTOs;

namespace DevJobs.Application.Features.JobPostAnalyzing.Services
{
    public class JobPostCountService: IJobPostCountService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public JobPostCountService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<JobPostCountDTO> GetJobPostCountByDays(int days)
        {
            var startDate = DateTime.Today.AddDays(-days+1);

            var jobPosts = _unitOfWork.JobPostRepository.GetAll().Where(x => x.PublishedOn > startDate).OrderBy(x => x.PublishedOn).GroupBy(x => x.PublishedOn).Select(x => new JobPostCountDTO{ Date = x.Key.ToString("dd-MM-yyyy"), JobCount = x.Count() }).ToList();

            //Adding missing dates with 0 count           
            var date = startDate;
            IList<JobPostCountDTO> JobPostCounts = new List<JobPostCountDTO>();

            while (date <= DateTime.Today)
            {
                var count = jobPosts.Where(x => x.Date == date.ToString("dd-MM-yyyy")).FirstOrDefault()?.JobCount??0;
                JobPostCounts.Add(new JobPostCountDTO { Date = date.ToString("dd-MM-yyyy"), JobCount=count});
                date = date.AddDays(1);
            }

            return JobPostCounts;
        }
    }
}
