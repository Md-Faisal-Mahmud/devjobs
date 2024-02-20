using DevJobs.Application.Features.Membership.DTOs;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;
using Microsoft.AspNetCore.Identity;

namespace DevJobs.Application.Features.Membership.Services
{
    public interface IUserManagementService
    {
        Task<IdentityResult> CreateUserAsync(UserCreateDTO user);
        Task<IdentityResult> UpdateUserAsync(UserDetailsDTO user);
        Task<IdentityResult> DeleteUserByIdAsync(Guid id);
        Task<IPaginate<UserListDTO>> GetUsersAsync(SearchRequest request);
        Task<UserDetailsDTO> GetUserByIdAsync(Guid id);
    }
}
