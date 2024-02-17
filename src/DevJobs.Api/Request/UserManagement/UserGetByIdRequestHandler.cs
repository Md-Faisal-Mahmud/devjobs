using Autofac;
using DevJobs.Application.Features.Membership.DTOs;
using DevJobs.Application.Features.Membership.Services;

namespace DevJobs.Api.Request.UserManagement
{
    public class UserGetByIdRequestHandler
    {
        public Guid Id { get; set; }

        private IUserManagementService _userManagementService;

        public UserGetByIdRequestHandler()
        {

        }

        public UserGetByIdRequestHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _userManagementService = scope.Resolve<IUserManagementService>();
        }
        public async Task<UserDetailsDTO> GetUserByIdAsync()
        {
            return await _userManagementService.GetUserByIdAsync(Id);
        }
    }
}
