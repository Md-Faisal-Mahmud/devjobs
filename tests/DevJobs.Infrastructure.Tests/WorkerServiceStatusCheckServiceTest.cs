using Autofac.Extras.Moq;
using DevJobs.Application.Features.Services;
using DevJobs.Infrastructure.Features.Services;
using DevJobs.Infrastructure.Utilities;
using Moq;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace DevJobs.Infrastructure.Tests
{
    [ExcludeFromCodeCoverage]
    public class WorkerServiceStatusCheckServiceTest
    {
        private AutoMock _mock;
        private Mock<IServiceStatus> _statusMock;
        private IWorkerServiceStatusCheckService _workerServiceStatusCheckService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _statusMock = _mock.Mock<IServiceStatus>();
            _workerServiceStatusCheckService = _mock.Create<WorkerServiceStatusCheckService>();
        }

        [TearDown]
        public void TearDown()
        {
            _statusMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public void CheckServiceStatus_StatusReturnsTrue_ReturnsTrue()
        {
            _statusMock.Setup(x => x.status()).Returns(true);

            var result = _workerServiceStatusCheckService.CheckServiceStatus();

            result.ShouldBeTrue();
        }

        [Test]
        public void CheckServiceStatus_StatusReturnsFalse_ReturnsFalse()
        {
            _statusMock.Setup(x => x.status()).Returns(false);

            var result = _workerServiceStatusCheckService.CheckServiceStatus();

            result.ShouldBeFalse();
        }
    }
}
