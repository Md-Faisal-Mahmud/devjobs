using Autofac.Extras.Moq;
using DevJobs.Application.Features.Services;
using DevJobs.Application.Utilities;
using DevJobs.Infrastructure.Features.Membership;
using DevJobs.Infrastructure.Features.Services;
using DevSkill.Http.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;

namespace DevJobs.Infrastructure.Tests
{
    public class ResetPasswordServiceTests
    {
        private AutoMock _mock;
        private Mock<IUserManagerAdapter<ApplicationUser, Guid>> _userManagerAdapterMock;
        private Mock<IAccountUtilities> _accountUtilitiesMock;
        private IResetPasswordService _resetPasswordService;

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
            _resetPasswordService = _mock.Create<ResetPasswordService>();
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
        public async Task VerifyResetPasswordRequest_ValidRequest_ReturnsTrue()
        {
            // Arrange
            var userId = "66c3aef9-9444-4c71-2db4-08dc050745ff";
            var code = "eyJhbGciOiJIUzI1NiIsInR5cCI6I";
            var password = "123456";
            var confirmPassword = "123456";

            var user = new ApplicationUser
            {
                Id = new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff"),
                FirstName = "Mr",
                LastName = "Admin"
            };
            _userManagerAdapterMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);
            _accountUtilitiesMock.Setup(x => x.DecodePasswordResetToken(code))
                .Returns("decodedToken");
            _userManagerAdapterMock.Setup(x => x.ResetPasswordAsync(user, "decodedToken", password))
                .ReturnsAsync(IdentityResult.Success);
            _accountUtilitiesMock.Setup(x => x.GetTrueReturn())
                .Returns("TrueReturn");

            // Act
            var result = await _resetPasswordService.VerifyResetPasswordRequest(userId, code, password, confirmPassword);

            // Assert
            result.ShouldBe("TrueReturn");
        }

        [Test]
        public async Task VerifyResetPasswordRequest_InvalidRequest_ReturnsFalse()
        {
            // Arrange
            var userId = "66c3aef9-9444-4c71-2db4-08dc050745ff";
            var code = "eyJhbGciOiJIUzI1NiIsInR5cCI6I";
            var password = "123456";
            var confirmPassword = "11111111111111111111111111";

            var user = new ApplicationUser
            {
                Id = new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff"),
                FirstName = "Mr",
                LastName = "Admin"
            };
            _userManagerAdapterMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);
            _accountUtilitiesMock.Setup(x => x.DecodePasswordResetToken(code))
                .Returns("decodedToken");
            _userManagerAdapterMock.Setup(x => x.ResetPasswordAsync(user, "decodedToken", password))
                .ReturnsAsync(IdentityResult.Success);
            _accountUtilitiesMock.Setup(x => x.GetFalseReturn())
                .Returns("FalseReturn");

            // Act
            var result = await _resetPasswordService.VerifyResetPasswordRequest(userId, code, password, confirmPassword);

            // Assert
            result.ShouldBe("FalseReturn");
        }
    }
}