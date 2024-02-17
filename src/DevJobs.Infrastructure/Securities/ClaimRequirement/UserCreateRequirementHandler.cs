using Microsoft.AspNetCore.Authorization;

namespace DevJobs.Infrastructure.Securities.ClaimRequirement
{
    public class UserCreateRequirementHandler: AuthorizationHandler<UserCreateRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserCreateRequirement requirement)
        {
            if(context.User.HasClaim(x=>x.Type == ClaimTypes.UserCreateClaim.Name 
            && x.Value == ClaimTypes.UserCreateClaim.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
