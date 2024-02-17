using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace DevJobs.Infrastructure.Features.Membership
{
    [ExcludeFromCodeCoverage]
    public class ApplicationRole: IdentityRole<Guid>
    {
        public ApplicationRole():base() { }
        public ApplicationRole(string roleName): base(roleName) { }
    }
}
