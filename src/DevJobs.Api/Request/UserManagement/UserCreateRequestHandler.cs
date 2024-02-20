using Autofac;
using DevJobs.Application.Features.Membership.DTOs;
using DevJobs.Application.Features.Membership.Services;
using Microsoft.AspNetCore.Identity;

namespace DevJobs.Api.Request.UserManagement
{
    public class UserCreateRequestHandler
    {
        private IUserManagementService _userManagementService;

        public UserCreateRequestHandler()
        {

        }

        public UserCreateRequestHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _userManagementService = scope.Resolve<IUserManagementService>();
        }

        public async Task<IdentityResult> CreateUser(UserCreateDTO user)
        {
            return await _userManagementService.CreateUserAsync(user);
        }
    }
}
