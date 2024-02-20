using Autofac.Extras.Moq;
using DevJobs.Application.Features.Services;
using DevJobs.Application.Utilities;
using DevJobs.Infrastructure.Features.Membership;
using DevJobs.Infrastructure.Features.Services;
using DevJobs.Infrastructure.Securities;
using DevSkill.Http.Membership;
using Moq;
using Shouldly;
using System.Security.Claims;

namespace DevJobs.Infrastructure.Tests
{
    [TestFixture]
    public class LoginServiceTests
    {
        private AutoMock _mock;
        private Mock<IUserManagerAdapter<ApplicationUser, Guid>> _userManagerAdapterMock;
        private Mock<ISignInManagerAdapter<ApplicationUser, Guid>> _signInManagerAdapterMock;
        private Mock<IAccountUtilities> _accountUtilitiesMock;
        private Mock<ITokenService> _tokenServiceMock;
        private ILoginService _loginService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _userManagerAdapterMock = _mock.Mock<IUserManagerAdapter<ApplicationUser, Guid>>();
            _signInManagerAdapterMock = _mock.Mock<ISignInManagerAdapter<ApplicationUser, Guid>>();
            _accountUtilitiesMock = _mock.Mock<IAccountUtilities>();
            _tokenServiceMock = _mock.Mock<ITokenService>();
            _loginService = _mock.Create<LoginService>();
        }

        [TearDown]
        public void Teardown()
        {
            _userManagerAdapterMock.Reset();
            _signInManagerAdapterMock.Reset();
            _accountUtilitiesMock.Reset();
            _tokenServiceMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public async Task LoginUserAsync_ValidCredentials_ReturnsLoginReturn()
        {
            // Arrange
            var emailAddress = "xyz@devjobs.com";
            var userName = "xyz@devjobs.com";
            var password = "123456";
            var rememberMe = false;

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = "Mr",
                LastName = "Admin",
                UserName = userName,
                Email = emailAddress,
                ImageName = "imageName"
            };

            _userManagerAdapterMock.Setup(x => x.FindByEmailAsync(emailAddress))
                .ReturnsAsync(user);
            _signInManagerAdapterMock.Setup(x => x.PasswordSignInAsync(emailAddress, password, rememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _userManagerAdapterMock.Setup(x => x.GetClaimsAsync(user))
                .ReturnsAsync(new List<Claim>());
            _userManagerAdapterMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin" });
            _tokenServiceMock.Setup(x => x.GetJwtToken(It.IsAny<Claim[]>()))
                .ReturnsAsync("jwtToken");
            _accountUtilitiesMock.Setup(x => x.GetImageUrl(user.ImageName))
                .Returns("imageData");
            _accountUtilitiesMock.Setup(x => x.GetRolesString(It.IsAny<IEnumerable<string>>()))
                .Returns("Admin");
            _accountUtilitiesMock.Setup(x => x.GetLoginReturn("jwtToken", "Mr", "Admin", userName, emailAddress, "Admin", "imageData"))
                .Returns("LoginReturn");

            // Act
            var result = await _loginService.LoginUserAsync(emailAddress, password, rememberMe);

            // Assert
            result.ShouldBe("LoginReturn");
        }

        [Test]
        public async Task LoginUserAsync_InvalidCredentials_ReturnsFalseReturn()
        {
            // Arrange
            var emailAddress = "xyz@devjobs.com";
            var password = "1234568888888888";
            var rememberMe = false;

            _signInManagerAdapterMock.Setup(x => x.PasswordSignInAsync(emailAddress, password, rememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
            _accountUtilitiesMock.Setup(x => x.GetFalseReturn())
                .Returns("FalseReturn");

            // Act
            var result = await _loginService.LoginUserAsync(emailAddress, password, rememberMe);

            // Assert
            result.ShouldBe("FalseReturn");
        }
    }
}