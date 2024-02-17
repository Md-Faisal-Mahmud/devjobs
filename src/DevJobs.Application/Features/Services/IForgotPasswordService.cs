using DevJobs.Application.DTOs;

namespace DevJobs.Application.Features.Services
{
    public interface IForgotPasswordService
    {
        Task<ForgotPasswordDTO> GeneratePasswordResetToken(string email, string origin);
    }
}
