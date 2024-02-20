using DevJobs.Domain.Entities;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;

namespace DevJobs.Application.Features.OrganizationList
{
    public interface IOrganizationListQueryService
    {
        Task<IPaginate<Organization>> GetOrganizationList(SearchRequest searchRequest);
    }
}
