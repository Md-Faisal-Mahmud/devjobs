using Autofac;
using DevJobs.Application.Features.Membership.DTOs;
using DevJobs.Application.Features.Membership.Services;
using Microsoft.AspNetCore.Identity;

namespace DevJobs.Api.Request.UserManagement
{
    public class UserUpdateRequestHandler
    {
        private IUserManagementService _userManagementService;

        public UserUpdateRequestHandler()
        {

        }

        public UserUpdateRequestHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _userManagementService = scope.Resolve<IUserManagementService>();
        }

        public async Task<IdentityResult> UpdateUser(UserDetailsDTO user)
        {
            return await _userManagementService.UpdateUserAsync(user);
        }
    }
}
