using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace DevJobs.Infrastructure.Features.Membership
{
    [ExcludeFromCodeCoverage]
    public class ApplicationUser: IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string ImageName { get; set; } = string.Empty;
    }
}
