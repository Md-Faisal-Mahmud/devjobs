using AutoMapper;
using DevJobs.Application.Features.Membership.DTOs;
using DevJobs.Application.Features.Membership.Services;
using DevJobs.Application.Services;
using DevJobs.Domain.Utilities;
using DevSkill.Extensions.FileStorage.Options;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Paginate.Extensions;
using DevSkill.Extensions.Queryable;
using DevSkill.Http.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DevJobs.Infrastructure.Features.Membership.Services
{
    public class UserManagementService : IUserManagementService
    {
        private IUserManagerAdapter<ApplicationUser, Guid> _userManagerAdapter;
        private IMapper _mapper;
        private IFileService _fileService;
        private FileStorageSetting _storageSettings;

        public UserManagementService(IUserManagerAdapter<ApplicationUser, Guid> userManagerAdapter,
            IMapper mapper,
            IFileService fileService,
            IOptions<FileStorageSetting> storageSettings) 
        {
            _userManagerAdapter = userManagerAdapter;
            _mapper = mapper;
            _fileService = fileService;
            _storageSettings = storageSettings.Value;
        }

        public async Task<IdentityResult> CreateUserAsync(UserCreateDTO userDTO)
        {
            var applicationUser = _mapper.Map<ApplicationUser>(userDTO);

            applicationUser.Id = Guid.NewGuid();
            var url = await _fileService.UploadFile(userDTO.Image, AWSFolder.ProfileImages);
            applicationUser.ImageName = Path.GetFileName(url);

            return await _userManagerAdapter.CreateAsync(applicationUser, userDTO.Password);           
        }

        public async Task<IdentityResult> DeleteUserByIdAsync(Guid id)
        {
            var user = await _userManagerAdapter.FindByIdAsync(id.ToString());

            if(user is not null)
            {
                if (!string.IsNullOrWhiteSpace(user.ImageName))
                {
                    await _fileService.DeleteFile($"{_storageSettings.FolderPaths[AWSFolder.ProfileImages]}/{user.ImageName}");
                }

              return  await _userManagerAdapter.DeleteAsync(user);
            }

            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        public async Task<UserDetailsDTO> GetUserByIdAsync(Guid id)
        {
            var user= await _userManagerAdapter.FindByIdAsync(id.ToString());

            if (user == null) return null;

            var dto = _mapper.Map<UserDetailsDTO>(user);

            if (!string.IsNullOrWhiteSpace(user.ImageName))
            {
                dto.ImageUrl = _fileService.GetFileUrl($"{_storageSettings.FolderPaths[AWSFolder.ProfileImages]}/{user.ImageName}", $"{_storageSettings.FolderPaths[AWSFolder.ProfileImages]}/{user.ImageName}");
            }

            return dto;
        }

        public async Task<IPaginate<UserListDTO>> GetUsersAsync(SearchRequest request)
        {
            var totalUsers = _userManagerAdapter.Users.Count();

            var users = new List<UserListDTO>();

            await Task.Run(() => {
               foreach(var  user in _userManagerAdapter.Users)
                {
                    var userdto = _mapper.Map<UserListDTO>(user);
                    users.Add(userdto);
                }
            }); 

            var filtered = users.FilterBy(request.Filters);

            foreach (var user in filtered)
            {
                if (!string.IsNullOrWhiteSpace(user.ImageUrl))
                {
                  user.ImageUrl = _fileService.GetFileUrl($"{_storageSettings.FolderPaths[AWSFolder.ProfileImages]}/{user.ImageUrl}", $"{_storageSettings.FolderPaths[AWSFolder.ProfileImages]}/{user.ImageUrl}");
                }
            }

            var ordered = filtered.OrderBy(request.SortOrders);
            var paginated = ordered.ToPaginate(request.PageIndex, request.PageSize, totalUsers, 1);
            return paginated;
        }

        public async Task<IdentityResult> UpdateUserAsync(UserDetailsDTO userDtails)
        {
            var user = await _userManagerAdapter.FindByIdAsync(userDtails.Id.ToString());
            if(user is not null)
            {
                if (!string.IsNullOrWhiteSpace(user.ImageName))
                {
                    await _fileService.DeleteFile($"{_storageSettings.FolderPaths[AWSFolder.ProfileImages]}/{user.ImageName}");
                }

                _mapper.Map(userDtails, user);

                if(!string.IsNullOrWhiteSpace(userDtails.Image))
                {
                    var imageName = Path.GetFileName(await _fileService.UploadFile(userDtails.Image, AWSFolder.ProfileImages));
                    user.ImageName = imageName;
                }

                return await _userManagerAdapter.UpdateAsync(user);
            }

            return IdentityResult.Failed([new IdentityError { Description="User not Found." }]);;
            
        }
    }
}
