using DevJobs.Domain.Entities;
using DevSkill.Data;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;

namespace DevJobs.Domain.Repositories
{
    public interface IDbLogRepository : IRepository<DbLog, int>
    {
        Task<IPaginate<DbLog>> GetPaginatedDbLogsAsync(SearchRequest request);        
        Task DeleteLogById(int id);
    }
}
