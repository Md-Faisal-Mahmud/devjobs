using Microsoft.AspNetCore.Authorization;

namespace DevJobs.Infrastructure.Securities.ClaimRequirement
{
    public class UserViewRequirementHandler: AuthorizationHandler<UserViewRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserViewRequirement requirement)
        {
            if(context.User.HasClaim(x=>x.Type == ClaimTypes.UserViewClaim.Name 
            && x.Value == ClaimTypes.UserViewClaim.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
