using Autofac.Extras.Moq;
using AutoMapper;
using DevJobs.Application.Features.Membership.DTOs;
using DevJobs.Application.Services;
using DevJobs.Infrastructure.Features.Membership;
using DevJobs.Infrastructure.Features.Membership.Services;
using DevSkill.Extensions.FileStorage.Options;
using DevSkill.Extensions.Queryable;
using DevSkill.Http.Membership;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;

namespace DevJobs.Infrastructure.Tests
{
    public class UserManagementServiceTest
    {
        private AutoMock _mock;
        private Mock<IUserManagerAdapter<ApplicationUser, Guid>> _userManagerAdapterMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IMapper> _mapperMock;
        private UserManagementService _userManagementService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _userManagerAdapterMock = _mock.Mock<IUserManagerAdapter<ApplicationUser, Guid>>();
            _mapperMock = _mock.Mock<IMapper>();
            _fileServiceMock = _mock.Mock<IFileService>();
            _userManagementService = _mock.Create<UserManagementService>();
        }

        [TearDown]
        public void Teardown()
        {
            _userManagerAdapterMock.Reset();
            _mapperMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public async Task CreateUser_ValidRequest_ReturnSuccess()
        {
            //Arrange
            var user = new UserCreateDTO
            {
                FirstName = "Abul",
                LastName = "Kalam",
                UserName = "kalam@gmail.com",
                Email = "kalam@gmail.com",
                PhoneNumber = "1234567890",
                Password = "123456"
            };

            var appUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = "Abul",
                LastName = "Kalam",
                UserName = "kalam@gmail.com",
                Email = "kalam@gmail.com",
                PhoneNumber = "1234567890",
            };

            _mapperMock.Setup(m => m.Map<ApplicationUser>(user)).Returns(appUser);
            _userManagerAdapterMock.Setup(m => m.CreateAsync(appUser, user.Password)).ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await _userManagementService.CreateUserAsync(user);

            //Assert
            result.Succeeded.ShouldBeTrue();
        }

        [Test]
        public async Task CreateUser_InValidRequest_ReturnFailure()
        {
            //Arrange
            var user = new UserCreateDTO
            {
                FirstName = "Abul",
                LastName = "Kalam",
                UserName = "kalam@gmail.com",
                Email = "kalam@gmail.com",
                PhoneNumber = "1234567890",
                Password = "123456"
            };

            var appUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = "Abul",
                LastName = "Kalam",
                UserName = "kalam@gmail.com",
                Email = "kalam@gmail.com",
                PhoneNumber = "1234567890",
            };

            _mapperMock.Setup(m => m.Map<ApplicationUser>(user)).Returns(appUser);
            _userManagerAdapterMock.Setup(m => m.CreateAsync(appUser, user.Password)).ReturnsAsync(IdentityResult.Failed([new IdentityError { Description = "Username already exist" }]));

            //Act
            var result = await _userManagementService.CreateUserAsync(user);

            //Assert
            result.Succeeded.ShouldBeFalse();
            result.Errors?.FirstOrDefault()?.Description?.ShouldBe("Username already exist");
        }

        [Test]
        public async Task DeleteUser_UserExist_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "Abul",
                LastName = "Kalam",
                UserName = "kalam@gmail.com",
                Email = "kalam@gmail.com",
                PhoneNumber = "1234567890",
            };

            _userManagerAdapterMock.Setup(u => u.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerAdapterMock.Setup(u => u.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userManagementService.DeleteUserByIdAsync(userId);

            // Assert
            result.Succeeded.ShouldBeTrue();
            _userManagerAdapterMock.Verify(u => u.DeleteAsync(user), Times.Once);
        }

        [Test]
        public async Task DeleteUser_NotFound_Failed()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userManagerAdapterMock.Setup(u => u.FindByIdAsync(userId.ToString())).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _userManagementService.DeleteUserByIdAsync(userId);

            // Assert
            result.Succeeded.ShouldBeFalse();
            result.Errors?.FirstOrDefault()?.Description?.ShouldBe("User not found.");
            _userManagerAdapterMock.Verify(u => u.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Test]
        public async Task GetUserById_UserExist_ReturnUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var appUser = new ApplicationUser
            {
                Id = userId,
                FirstName = "Abul",
                LastName = "Kalam",
                UserName = "kalam@gmail.com",
                Email = "kalam@gmail.com",
                PhoneNumber = "1234567890",
            };

            var userDto = new UserDetailsDTO
            {
                Id = userId,
                FirstName = "Abul",
                LastName = "Kalam",
                UserName = "kalam@gmail.com",
                Email = "kalam@gmail.com",
                PhoneNumber = "1234567890",
            };

            _userManagerAdapterMock.Setup(u => u.FindByIdAsync(userId.ToString())).ReturnsAsync(appUser);
            _mapperMock.Setup(m => m.Map<UserDetailsDTO>(appUser)).Returns(userDto);

            // Act
            var result = await _userManagementService.GetUserByIdAsync(userId);

            // Assert
            _userManagerAdapterMock.Verify(u => u.FindByIdAsync(userId.ToString()), Times.Once);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(userDto.Id);
            result.UserName.ShouldBe(userDto.UserName);
            result.Email.ShouldBe(userDto.Email);
        }

        [Test]
        public async Task GetUserById_UserNotFound_ReturnNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var appUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = "Abul",
                LastName = "Kalam",
                UserName = "kalam@gmail.com",
                Email = "kalam@gmail.com",
                PhoneNumber = "1234567890",
            };

            _userManagerAdapterMock.Setup(u => u.FindByIdAsync(userId.ToString())).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _userManagementService.GetUserByIdAsync(userId);

            // Assert
            _userManagerAdapterMock.Verify(u => u.FindByIdAsync(userId.ToString()), Times.Once);
            _mapperMock.Verify(m => m.Map<UserDetailsDTO>(It.IsAny<ApplicationUser>()), Times.Never);
            result.ShouldBeNull();
        }

        [Test]
        public async Task GetUsersAsync_Success()
        {
            // Arrange
            var request = new SearchRequest
            {
                Filters = new List<FilterColumn>(),
                SortOrders = new List<SortOrder>(),
                PageIndex = 1,
                PageSize = 10
            };

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = id1,
                    FirstName = "Abul",
                    LastName = "Kalam",
                    UserName = "kalam@gmail.com",
                    Email = "kalam@gmail.com",
                    PhoneNumber = "1234567890",
                },

                new ApplicationUser
                {
                    Id = id2,
                    FirstName = "Abdus",
                    LastName = "Salam",
                    UserName = "salam@gmail.com",
                    Email = "salam@gmail.com",
                    PhoneNumber = "1234567890",
                }
            };
            var usersDto = new List<UserListDTO>
            {
                new UserListDTO
                {
                    UserId = id1,
                    FirstName = "Abul",
                    LastName = "Kalam",
                    UserName = "kalam@gmail.com",
                    UserEmail = "kalam@gmail.com",
                    PhoneNumber = "1234567890",
                },

                new UserListDTO
                {
                    UserId = id2,
                    FirstName = "Abdus",
                    LastName = "Salam",
                    UserName = "salam@gmail.com",
                    UserEmail = "salam@gmail.com",
                    PhoneNumber = "1234567890",
                }
            };

            _userManagerAdapterMock.Setup(u => u.Users).Returns(users.AsQueryable<ApplicationUser>);

            _mapperMock.Setup(m => m.Map<UserListDTO>(It.IsAny<ApplicationUser>()))
              .Returns<ApplicationUser>(user =>
              {
                  var userDto = usersDto.FirstOrDefault(u => u.UserId == user.Id);
                  return userDto != null ? userDto : new UserListDTO(); // Handle null case if necessary
              });

            // Act
            var result = await _userManagementService.GetUsersAsync(request);

            // Assert
            result.ShouldNotBeNull();
            result.Total.ShouldBe(2);
            result.TotalFiltered.ShouldBe(2);
            result.Index.ShouldBe(1);
            result.Size.ShouldBe(10);
        }

        [Test]
        public async Task UpdateUserAsync_UserExists()
        {
            // Arrange
            var userDetailsDto = new UserDetailsDTO
            {
                Id = Guid.NewGuid(),
                FirstName = "Abul",
                LastName = "Kalam",
                UserName = "kalam@gmail.com",
                Email = "kalam@gmail.com",
                PhoneNumber = "1234567890",
            };

            var user = new ApplicationUser
            {
                Id = userDetailsDto.Id,
                FirstName = "Abul",
                LastName = "Kalam",
                UserName = "kalam@gmail.com",
                Email = "kalam@gmail.com",
                PhoneNumber = "0123456789",
            };

            _userManagerAdapterMock.Setup(u => u.FindByIdAsync(userDetailsDto.Id.ToString())).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<ApplicationUser>(userDetailsDto)).Returns(user);
            _userManagerAdapterMock.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userManagementService.UpdateUserAsync(userDetailsDto);

            // Assert
            result.Succeeded.ShouldBeTrue();
            _userManagerAdapterMock.Verify(u => u.UpdateAsync(user), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_UserNotFound_Error()
        {
            // Arrange
            var userDetailsDto = new UserDetailsDTO
            {
                Id = Guid.NewGuid(),
                FirstName = "Abul",
                LastName = "Kalam",
                UserName = "kalam@gmail.com",
                Email = "kalam@gmail.com",
                PhoneNumber = "1234567890",
            };

            _userManagerAdapterMock.Setup(u => u.FindByIdAsync(userDetailsDto.Id.ToString())).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _userManagementService.UpdateUserAsync(userDetailsDto);

            // Assert
            result.Succeeded.ShouldBeFalse();
            result.Errors?.FirstOrDefault()?.Description?.ShouldBe("User not Found.");
            _userManagerAdapterMock.Verify(u => u.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }
    }
}
