using DevJobs.Application.DTOs;
using DevJobs.Application.Features.Services;
using DevJobs.Application.Utilities;
using DevJobs.Infrastructure.Features.Membership;
using DevSkill.Http.Membership;

namespace DevJobs.Infrastructure.Features.Services
{
    public class ForgotPasswordService(
        IAccountUtilities accountUtilities,
        IUserManagerAdapter<ApplicationUser, Guid> userManagerAdapter) : IForgotPasswordService
    {
        public async Task<ForgotPasswordDTO> GeneratePasswordResetToken(string email, string origin)
        {
            var user = await userManagerAdapter.FindByEmailAsync(email);

            if (user != null)
            {
                var code = await userManagerAdapter.GeneratePasswordResetTokenAsync(user);
                code = accountUtilities.EncodePasswordResetToken(code);
                var callbackUrl = accountUtilities.GenerateCallbackUrl(origin, user.Id.ToString(), code);
                return accountUtilities.ForgotPasswordReturn(true, user.FirstName!, user.LastName!, email, callbackUrl);
            }
            return accountUtilities.ForgotPasswordReturn(false, string.Empty, string.Empty, string.Empty, string.Empty);
        }
    }
}