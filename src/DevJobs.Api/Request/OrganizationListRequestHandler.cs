using Autofac;
using DevJobs.Application.Features.JobPostAnalyzing.Services;
using DevJobs.Application.Features.OrganizationList;
using DevJobs.Domain.Entities;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace DevJobs.Api.Request
{
    public class OrganizationListRequestHandler
    {
        private IOrganizationListQueryService _organizationListQueryService;

        public OrganizationListRequestHandler()
        {
            
        }

        public OrganizationListRequestHandler(IOrganizationListQueryService organizationListQueryService)
        {
            _organizationListQueryService = organizationListQueryService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _organizationListQueryService = scope.Resolve<IOrganizationListQueryService>();
        }

        public async Task<IPaginate<Organization>> GetOrganizationListAsync(SearchRequest searchRequest)
        {
            return await _organizationListQueryService.GetOrganizationList(searchRequest);
        }
    }
}
