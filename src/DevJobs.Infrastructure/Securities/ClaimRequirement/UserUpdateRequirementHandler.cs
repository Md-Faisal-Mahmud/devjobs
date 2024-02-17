using Microsoft.AspNetCore.Authorization;

namespace DevJobs.Infrastructure.Securities.ClaimRequirement
{
    public class UserUpdateRequirementHandler : AuthorizationHandler<UserUpdateRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserUpdateRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == ClaimTypes.UserUpdateClaim.Name
            && x.Value == ClaimTypes.UserUpdateClaim.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
