using Autofac;
using DevJobs.Application.Features.DbLogViewing;
using DevJobs.Domain.Entities;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;

namespace DevJobs.Api.Request
{
    public class DbLogListRequestHandler
    {
        private IDbLogQueryService _dbLogQueryService;

        public DbLogListRequestHandler()
        {

        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _dbLogQueryService = scope.Resolve<IDbLogQueryService>();
        }

        public async Task<IPaginate<DbLog>> GetPaginatedDbLogsAsync(SearchRequest request)
        {
            return await _dbLogQueryService.GetPaginatedDbLogsAsync(request);
        }
    }
}
