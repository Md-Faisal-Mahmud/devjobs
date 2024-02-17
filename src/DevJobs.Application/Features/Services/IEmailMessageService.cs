namespace DevJobs.Application.Features.Services
{
    public interface IEmailMessageService
    {
        Task<object> SendResetPasswordEmailAsync(bool isUserExist, string receiverName, string receiverEmail, string resetPasswordLink);
    }
}
