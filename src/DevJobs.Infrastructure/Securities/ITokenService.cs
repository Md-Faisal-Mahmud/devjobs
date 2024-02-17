using System.Security.Claims;

namespace DevJobs.Infrastructure.Securities
{
    public interface ITokenService
    {
        Task<string> GetJwtToken(IList<Claim> claims);
    }
}
