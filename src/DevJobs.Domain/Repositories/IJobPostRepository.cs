using DevJobs.Domain.Entities;
using DevSkill.Data;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;

namespace DevJobs.Domain.Repositories
{
    public interface IJobPostRepository : IRepository<JobPost, Guid>
    {
        Organization GetCompany(string companyName);
        Task<IPaginate<JobPost>> GetPaginatedJobPostsAsync(SearchRequest request);
        Task<JobPost> GetJobPostAsync(Guid id);
    }
}