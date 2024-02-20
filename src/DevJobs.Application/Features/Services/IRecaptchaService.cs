namespace DevJobs.Application.Features.Services
{
    public interface IRecaptchaService
    {
        Task<object> VerifyRecaptchaTokenAsync(string recaptchaToken);
    }
}
