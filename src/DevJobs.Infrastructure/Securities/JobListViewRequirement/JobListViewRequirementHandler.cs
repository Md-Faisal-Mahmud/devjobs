using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Infrastructure.Securities.JobListViewRequirement
{
    public class JobListViewRequirementHandler : AuthorizationHandler<JobListViewRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            JobListViewRequirement requirement)
        {

            if (context.User.HasClaim(x => x.Type == ClaimTypes.JobListViewClaim.Name && x.Value == ClaimTypes.JobListViewClaim.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
