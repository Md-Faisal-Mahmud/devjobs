using DevJobs.Domain.Entities;
using DevJobs.Domain.Repositories;
using DevSkill.Data;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Paginate.Extensions;
using DevSkill.Extensions.Queryable;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Infrastructure.Repositories
{
    public class DbLogRepository : Repository<DbLog, int>, IDbLogRepository
    {
        public DbLogRepository(IApplicationDbContext context) : base((DbContext)context)
        {

        }

        public async Task<IPaginate<DbLog>> GetPaginatedDbLogsAsync(SearchRequest request)
        {
            var totalCount = await GetCountAsync();
            var logs = Get(null, null, null);

            var result = await logs
                .FilterBy(request.Filters)
                .OrderBy(request.SortOrders)
                .ToPaginateAsync(request.PageIndex, request.PageSize, totalCount, 1);

            return result;
        }

        public async Task DeleteLogById(int id)
        {
            await RemoveAsync(id);
        }
    }
}
