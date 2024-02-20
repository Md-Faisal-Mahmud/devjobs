namespace DevJobs.Application.Features.Services
{
    public interface IResetPasswordService
    {
        Task<object> VerifyResetPasswordRequest(string userId, string code, string password, string confirmPassword);
    }
}
