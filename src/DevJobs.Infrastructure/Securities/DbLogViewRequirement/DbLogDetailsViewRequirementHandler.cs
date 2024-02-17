using Microsoft.AspNetCore.Authorization;

namespace DevJobs.Infrastructure.Securities.DbLogViewRequirement
{
    public class DbLogDetailsViewRequirementHandler : AuthorizationHandler<DbLogDetailsViewRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            DbLogDetailsViewRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "LogDetailsViewRequirementPolicy" && x.Value == "true"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
