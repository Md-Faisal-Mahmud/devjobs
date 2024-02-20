namespace DevJobs.Application.Features.JobParsing
{
    public interface IBdJobsParserService
    {
        Task ParseAllJobsAsync();
    }
}