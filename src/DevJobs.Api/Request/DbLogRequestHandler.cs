using Autofac;
using DevJobs.Application.Features.DbLogViewing;
using DevJobs.Domain.Entities;

namespace DevJobs.Api.Request
{
    public class DbLogRequestHandler
    {
        private IDbLogQueryService _dbLogQueryService;

        public DbLogRequestHandler(IDbLogQueryService dataCollectorLogQueryService)
        {
            _dbLogQueryService = dataCollectorLogQueryService;
        }

        public void ResolvedDependency(ILifetimeScope scope)
        {
            _dbLogQueryService = scope.Resolve<IDbLogQueryService>();
        }

        public async Task<DbLog>? GetLogAsync(int id)
        {
            return await _dbLogQueryService.GetDbLogAsync(id);
        }

        public async Task DeleteDbLogAsync(int id)
        {
            await _dbLogQueryService.DeleteDbLogAsync(id);
        }
    }
}
