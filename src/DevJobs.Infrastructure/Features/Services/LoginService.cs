using DevJobs.Application.Features.Services;
using DevJobs.Application.Utilities;
using DevJobs.Infrastructure.Features.Membership;
using DevJobs.Infrastructure.Securities;
using DevSkill.Http.Membership;

namespace DevJobs.Infrastructure.Features.Services
{
    public class LoginService(
        IUserManagerAdapter<ApplicationUser, Guid> userManagerAdapter,
        ISignInManagerAdapter<ApplicationUser, Guid> signInManagerAdapter,
        IAccountUtilities accountUtilities,
        ITokenService tokenService) : ILoginService
    {
        public async Task<object> LoginUserAsync(string emailaddress, string password, bool rememberMe)
        {
            var user = await userManagerAdapter.FindByEmailAsync(emailaddress);

            if (user == null)
                return accountUtilities.GetFalseReturn();

            var result = await signInManagerAdapter.PasswordSignInAsync(
                user.UserName, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var claims = (await userManagerAdapter.GetClaimsAsync(user)).ToArray();
                var token = await tokenService.GetJwtToken(claims);
                var roles = await userManagerAdapter.GetRolesAsync(user);

                var imageData = "";

                if (!string.IsNullOrWhiteSpace(user.ImageName))
                {
                    imageData = accountUtilities.GetImageUrl(user.ImageName);
                }
                
                var roleString = accountUtilities.GetRolesString(roles);
                return accountUtilities.GetLoginReturn(token, user.FirstName!, user.LastName!, user.UserName!, user.Email!, roleString, imageData);
            }
            else
                return accountUtilities.GetFalseReturn();
        }
    }
}
