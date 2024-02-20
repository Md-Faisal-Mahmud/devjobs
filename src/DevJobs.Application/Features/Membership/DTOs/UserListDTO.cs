
namespace DevJobs.Application.Features.Membership.DTOs
{
    public class UserListDTO
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}
