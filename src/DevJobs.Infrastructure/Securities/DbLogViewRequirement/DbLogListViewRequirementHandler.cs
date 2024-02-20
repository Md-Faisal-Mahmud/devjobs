using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Infrastructure.Securities.DbLogViewRequirement
{
    public class DbLogListViewRequirementHandler : AuthorizationHandler<DbLogListViewRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            DbLogListViewRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "LogListViewRequirementPolicy" && x.Value == "true"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }        
    }
}
