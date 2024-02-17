using Autofac;
using DevJobs.Application.Features.Membership.Services;
using Microsoft.AspNetCore.Identity;

namespace DevJobs.Api.Request.UserManagement
{
    public class UserDeleteByIdRequestHandler
    {
        public Guid Id { get; set; }

        private IUserManagementService _userManagementService;
        
        public UserDeleteByIdRequestHandler()
        {

        }

        public UserDeleteByIdRequestHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _userManagementService = scope.Resolve<IUserManagementService>();
        }

        public async Task<IdentityResult> DeleteUserById()
        {
            return await _userManagementService.DeleteUserByIdAsync(Id);
        }
    }
}
