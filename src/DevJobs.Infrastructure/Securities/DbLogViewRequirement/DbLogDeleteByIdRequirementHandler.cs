using Microsoft.AspNetCore.Authorization;

namespace DevJobs.Infrastructure.Securities.DbLogViewRequirement
{
    public class DbLogDeleteByIdRequirementHandler : AuthorizationHandler<DbLogDeleteByIdRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            DbLogDeleteByIdRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "DeleteLogByIdRequirementPolicy" && x.Value == "true"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
