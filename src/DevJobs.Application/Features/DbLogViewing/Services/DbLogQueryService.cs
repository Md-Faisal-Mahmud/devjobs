using DevJobs.Domain.Entities;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;
using Serilog;

namespace DevJobs.Application.Features.DbLogViewing.Services
{
    public class DbLogQueryService : IDbLogQueryService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public DbLogQueryService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DbLog> GetDbLogAsync(int id)
        {
            return await _unitOfWork.LogRepository.GetByIdAsync(id);
        }

        public async Task<IPaginate<DbLog>> GetPaginatedDbLogsAsync(SearchRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return await _unitOfWork.LogRepository.GetPaginatedDbLogsAsync(request);
        }

        public async Task DeleteDbLogAsync(int id)
        {
            await _unitOfWork.LogRepository.DeleteLogById(id); 
            await _unitOfWork.SaveAsync();
            
        }
    }
}
