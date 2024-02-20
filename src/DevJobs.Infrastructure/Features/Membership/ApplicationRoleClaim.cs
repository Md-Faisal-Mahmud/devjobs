using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace DevJobs.Infrastructure.Features.Membership
{
    [ExcludeFromCodeCoverage]
    public class ApplicationRoleClaim: IdentityRoleClaim<Guid>
    {
        public ApplicationRoleClaim(): base() { }
    }
}
