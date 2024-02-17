using Autofac.Extras.Moq;
using DevJobs.Application.DTOs;
using DevJobs.Application.Features.Services;
using DevJobs.Application.Utilities;
using DevJobs.Infrastructure.Features.Membership;
using DevJobs.Infrastructure.Features.Services;
using DevSkill.Http.Membership;
using Moq;
using Shouldly;

namespace DevJobs.Infrastructure.Tests
{
    [TestFixture]
    public class ForgotPasswordServiceTests
    {
        private AutoMock _mock;
        private Mock<IUserManagerAdapter<ApplicationUser, Guid>> _userManagerAdapterMock;
        private Mock<IAccountUtilities> _accountUtilitiesMock;
        private IForgotPasswordService _forgotPasswordService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _userManagerAdapterMock = _mock.Mock<IUserManagerAdapter<ApplicationUser, Guid>>();
            _accountUtilitiesMock = _mock.Mock<IAccountUtilities>();
            _forgotPasswordService = _mock.Create<ForgotPasswordService>();
        }

        [TearDown]
        public void Teardown()
        {
            _userManagerAdapterMock.Reset();
            _accountUtilitiesMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public async Task GeneratePasswordResetToken_UserExists_ReturnsForgotPasswordReturn()
        {
            // Arrange
            var email = "xyz@devjobs.com";
            var origin = "http://localhost:4200";
            var user = new ApplicationUser
            {
                Id = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                FirstName = "Mr",
                LastName = "XYZ",
                Email = email
            };

            _userManagerAdapterMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);
            _userManagerAdapterMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("resetToken");
            _accountUtilitiesMock.Setup(x => x.EncodePasswordResetToken("resetToken"))
                .Returns("encodedToken");
            _accountUtilitiesMock.Setup(x => x.GenerateCallbackUrl(origin, user.Id.ToString(), "encodedToken"))
                .Returns("callbackUrl");
            _accountUtilitiesMock.Setup(x => x.ForgotPasswordReturn(true, "Mr", "XYZ", email, "callbackUrl"))
                .Returns(new ForgotPasswordDTO
                {
                    IsSuccess = true,
                    FullName = "Mr XYZ",
                    Email = email,
                    CallbackUrl = "callbackUrl"
                });

            // Act
            var result = await _forgotPasswordService.GeneratePasswordResetToken(email, origin);

            // Assert
            result.IsSuccess.ShouldBe(true);
            result.FullName.ShouldBe("Mr XYZ");
            result.Email.ShouldBe(email);
            result.CallbackUrl.ShouldBe("callbackUrl");
        }

        [Test]
        public async Task GeneratePasswordResetToken_UserDoesNotExist_ReturnsForgotPasswordReturn()
        {
            // Arrange
            var email = "null@devjobs.com";
            var origin = "http://localhost:4200";

            _userManagerAdapterMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((ApplicationUser?)null);
            _accountUtilitiesMock.Setup(x => x.ForgotPasswordReturn(false, string.Empty, string.Empty, string.Empty, string.Empty))
                .Returns(new ForgotPasswordDTO
                {
                    IsSuccess = false,
                    FullName = string.Empty,
                    Email = string.Empty,
                    CallbackUrl = string.Empty
                });

            // Act
            var result = await _forgotPasswordService.GeneratePasswordResetToken(email, origin);

            // Assert
            result.IsSuccess.ShouldBe(false);
            result.FullName.ShouldBe(string.Empty);
            result.Email.ShouldBe(string.Empty);
            result.CallbackUrl.ShouldBe(string.Empty);
        }
    }
}
