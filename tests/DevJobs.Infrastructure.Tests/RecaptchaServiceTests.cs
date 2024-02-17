using Autofac.Extras.Moq;
using DevJobs.Application.Features.Services;
using DevJobs.Application.Utilities;
using DevJobs.Infrastructure.Features.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;

namespace DevJobs.Infrastructure.Tests
{
    [TestFixture]
    public class RecaptchaServiceTests
    {
        private AutoMock _mock;
        private Mock<IAccountUtilities> _accountUtilitiesMock;
        private Mock<IConfiguration> _configurationMock;
        private IRecaptchaService _recaptchaService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _accountUtilitiesMock = _mock.Mock<IAccountUtilities>();
            _configurationMock = _mock.Mock<IConfiguration>();
            _recaptchaService = _mock.Create<RecaptchaService>();
        }

        [TearDown]
        public void Teardown()
        {
            _accountUtilitiesMock.Reset();
            _configurationMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public async Task VerifyRecaptchaTokenAsync_ValidToken_ReturnsTrue()
        {
            // Arrange
            var recaptchaToken = "1SF2332SDFD";
            _configurationMock.Setup(x => x["ReCaptcha:SecretKey"])
                .Returns("validSecretKey");
            _accountUtilitiesMock.Setup(x => x.VerifyRecaptchaToken("validSecretKey", recaptchaToken))
                .ReturnsAsync(true);
            _accountUtilitiesMock.Setup(x => x.GetTrueReturn())
                .Returns("TrueReturn");

            // Act
            var result = await _recaptchaService.VerifyRecaptchaTokenAsync(recaptchaToken);

            // Assert
            result.ShouldBe("TrueReturn");
        }

        [Test]
        public async Task VerifyRecaptchaTokenAsync_InvalidToken_ReturnsFalse()
        {
            // Arrange
            var recaptchaToken = "12ASD";
            _configurationMock.Setup(x => x["ReCaptcha:SecretKey"])
                .Returns("validSecretKey");
            _accountUtilitiesMock.Setup(x => x.VerifyRecaptchaToken("validSecretKey", recaptchaToken))
                .ReturnsAsync(false);
            _accountUtilitiesMock.Setup(x => x.GetFalseReturn())
                .Returns("FalseReturn");

            // Act
            var result = await _recaptchaService.VerifyRecaptchaTokenAsync(recaptchaToken);

            // Assert
            result.ShouldBe("FalseReturn");
        }
    }
}
