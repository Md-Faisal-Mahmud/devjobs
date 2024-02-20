using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Infrastructure.Securities.JobDetailsViewRequirement
{
    public class JobDetailsViewRequirementHandler : AuthorizationHandler<JobDetailsViewRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            JobDetailsViewRequirement requirement)
        {
            if(context.User.HasClaim(x=>x.Type == ClaimTypes.DetailsJobViewClaim.Name && x.Value==ClaimTypes.DetailsJobViewClaim.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
