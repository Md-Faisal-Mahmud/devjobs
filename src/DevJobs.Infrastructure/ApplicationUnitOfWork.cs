using DevJobs.Application;
using DevJobs.Domain.Repositories;
using DevSkill.Data;
using Microsoft.EntityFrameworkCore;

namespace DevJobs.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IJobPostRepository JobPostRepository { get; private set; }
        public ITrackMappingRepository TrackMappingRepository { get; private set; }
        public ITechnologyRepository TechnologyRepository { get; private set; }
        public IExperienceMappingRepository ExperienceMappingRepository { get; private set; }
        public IDbLogRepository LogRepository {get; private set;}

        public ApplicationUnitOfWork(IApplicationDbContext dbContext,
            IJobPostRepository jobPostRepository,
            ITrackMappingRepository trackMappingRepository,
            ITechnologyRepository technologyRepository,
            IExperienceMappingRepository experienceMappingRepository,
            IDbLogRepository logRepository) : base((DbContext)dbContext)
        {
            JobPostRepository = jobPostRepository;
            TrackMappingRepository = trackMappingRepository;
            TechnologyRepository = technologyRepository;
            ExperienceMappingRepository = experienceMappingRepository;
            LogRepository = logRepository;
        }
    }
}