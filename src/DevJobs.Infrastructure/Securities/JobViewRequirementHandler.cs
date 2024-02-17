using Microsoft.AspNetCore.Authorization;

namespace DevJobs.Infrastructure.Securities
{
    public class JobViewRequirementHandler : AuthorizationHandler<JobViewRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, JobViewRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == ClaimTypes.OrganizationViewClaim.Name && x.Value == ClaimTypes.OrganizationViewClaim.Value))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;  
        }
    }
}
