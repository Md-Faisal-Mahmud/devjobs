using Microsoft.AspNetCore.Authorization;

namespace DevJobs.Infrastructure.Securities
{
    public class WorkerServiceStatusViewRequirementHandler :
        AuthorizationHandler<WorkerServiceStatusViewRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            WorkerServiceStatusViewRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == ClaimTypes.ServiceStatusViewClaim.Name && x.Value == ClaimTypes.ServiceStatusViewClaim.Value))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
