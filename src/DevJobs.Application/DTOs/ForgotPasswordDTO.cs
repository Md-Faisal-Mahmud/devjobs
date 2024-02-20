namespace DevJobs.Application.DTOs
{
    public class ForgotPasswordDTO
    {
        public bool IsSuccess { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string CallbackUrl { get; set; }
    }
}
