
using Microsoft.AspNetCore.Identity;

namespace DevJobs.Application.Features.Membership.DTOs
{
    public class UserDetailsDTO
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool PhoneNumberConfirmed { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
    }
}
