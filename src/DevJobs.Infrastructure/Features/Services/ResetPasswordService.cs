using DevJobs.Application.Features.Services;
using DevJobs.Application.Utilities;
using DevJobs.Infrastructure.Features.Membership;
using DevSkill.Http.Membership;

namespace DevJobs.Infrastructure.Features.Services
{
    public class ResetPasswordService(
        IAccountUtilities accountUtilities,
        IUserManagerAdapter<ApplicationUser, Guid> userManagerAdapter) : IResetPasswordService
    {
        public async Task<object> VerifyResetPasswordRequest(string userId, string code, string password, string confirmPassword)
        {
            var user = await userManagerAdapter.FindByIdAsync(userId);

            if (user != null && password == confirmPassword)
            {
                var decodedToken = accountUtilities.DecodePasswordResetToken(code);
                var result = await userManagerAdapter.ResetPasswordAsync(user, decodedToken, password);

                if (result.Succeeded)
                    return accountUtilities.GetTrueReturn();
            }
            return accountUtilities.GetFalseReturn();
        }
    }
}