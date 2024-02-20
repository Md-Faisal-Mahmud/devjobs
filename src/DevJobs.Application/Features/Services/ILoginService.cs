namespace DevJobs.Application.Features.Services
{
    public interface ILoginService
    {
        Task<object> LoginUserAsync(string emailaddress, string password, bool rememberMe);
    }
}
