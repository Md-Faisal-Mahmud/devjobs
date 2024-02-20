using DevJobs.Domain.Repositories;
using DevSkill.Data;

namespace DevJobs.Application
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        IJobPostRepository JobPostRepository { get; }
        ITrackMappingRepository TrackMappingRepository { get; }
        ITechnologyRepository TechnologyRepository { get; }
        IExperienceMappingRepository ExperienceMappingRepository { get; }
        IDbLogRepository LogRepository { get; }
    }
}