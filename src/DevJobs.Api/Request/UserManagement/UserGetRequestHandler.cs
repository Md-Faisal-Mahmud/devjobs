using Autofac;
using DevJobs.Application.Features.Membership.DTOs;
using DevJobs.Application.Features.Membership.Services;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;

namespace DevJobs.Api.Request.UserManagement
{
    public class UserGetRequestHandler
    {
        public Guid Id { get; set; }

        private IUserManagementService _userManagementService;

        public UserGetRequestHandler()
        {

        }

        public UserGetRequestHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _userManagementService = scope.Resolve<IUserManagementService>();
        }

        public async Task<IPaginate<UserListDTO>> GetUsersAsync(SearchRequest request)
        {
            return await _userManagementService.GetUsersAsync(request);
        }
    }
}
