using DevJobs.Domain.Entities;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Application.Features.DbLogViewing
{
    public interface IDbLogQueryService
    {
        Task<IPaginate<DbLog>> GetPaginatedDbLogsAsync(SearchRequest request);
        Task<DbLog> GetDbLogAsync(int id);
        Task DeleteDbLogAsync(int id);      
    }
}
