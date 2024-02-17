using DevJobs.Domain.Entities;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Paginate.Extensions;
using DevSkill.Extensions.Queryable;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DevJobs.Application.Features.OrganizationList
{
    public class OrganizationListQueryService : IOrganizationListQueryService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public OrganizationListQueryService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IPaginate<Organization>> GetOrganizationList(SearchRequest searchRequest)
        {
            if (searchRequest is null)
            {
                throw new ArgumentNullException(nameof(searchRequest));
            }

            Expression<Func<JobPost, bool>> filterExpression = jobPost => jobPost.Company != null;
            int total = await _unitOfWork.JobPostRepository.GetCountAsync(filterExpression);

            var result = _unitOfWork.JobPostRepository
                .Get(filter: filterExpression, include: q => q.Include(jp => jp.Company))
                .Select(c => c.Company).Distinct().ToList()
                .FilterBy(searchRequest.Filters)
                .OrderBy(searchRequest.SortOrders)
                .ToPaginate(searchRequest.PageIndex, searchRequest.PageSize, total, 1);
         
            return result;
        }
    }
}
