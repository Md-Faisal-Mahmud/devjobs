using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Infrastructure.Securities.ClaimRequirement
{
    public class UserDeleteRequirementHandler : AuthorizationHandler<UserDeleteRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserDeleteRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == ClaimTypes.UserDeleteClaim.Name
            && x.Value == ClaimTypes.UserDeleteClaim.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
