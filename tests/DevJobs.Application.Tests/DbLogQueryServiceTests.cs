using Autofac.Extras.Moq;
using DevJobs.Application.Features.DbLogViewing;
using DevJobs.Application.Features.DbLogViewing.Services;
using DevJobs.Application.Features.JobPostVisualizing.Services;
using DevJobs.Domain.Entities;
using DevJobs.Domain.Repositories;
using DevSkill.Extensions.Paginate;
using DevSkill.Extensions.Queryable;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Application.Tests
{
    [ExcludeFromCodeCoverage]
    public class DbLogQueryServiceTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
        private Mock<IDbLogRepository> _dbLogRepositoryMock;
        private IDbLogQueryService _dbLogQueryService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _applicationUnitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
            _dbLogRepositoryMock = _mock.Mock<IDbLogRepository>();
            _dbLogQueryService = _mock.Create<DbLogQueryService>();
        }

        [TearDown]
        public void TearDown()
        {
            _applicationUnitOfWorkMock.Reset();
            _dbLogRepositoryMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public async Task GetDbLogAsync_ValidId_ReturnsDbLog()
        {
            var logId = 1;

            DbLog expectedDbLog = new DbLog
            {
                Id = logId,
                Message = "Sample log message",
                Level = "Information",
                Timestamp = DateTime.UtcNow,
                Exception = null,
                LogEvent = "{\"Timestamp\":\"2024-02-07T23:44:06.6693866\",\"Level\":\"Information\",\"Message\":\"Application starting up!\",\"Properties\":{\"MachineName\":\"EMRUL\"}}",
                TraceId = null,
                SpanId = null
            };

            _applicationUnitOfWorkMock
                .Setup(x => x.LogRepository)
                .Returns(_dbLogRepositoryMock.Object);

            _dbLogRepositoryMock.
                Setup(x => x.GetByIdAsync(logId)).
                ReturnsAsync(expectedDbLog);

            var result = await _dbLogQueryService.GetDbLogAsync(logId);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(logId);
        }

        [Test]
        public async Task GetDbLogAsync_NonExistentId_ReturnsNull()
        {
            var nonExistentId = 999; 

            _applicationUnitOfWorkMock
                .Setup(x => x.LogRepository)
                .Returns(_dbLogRepositoryMock.Object);

            _dbLogRepositoryMock.
                Setup(x => x.GetByIdAsync(nonExistentId)).
                ReturnsAsync((DbLog)null);

            var result = await _dbLogQueryService.GetDbLogAsync(nonExistentId);

            result.ShouldBeNull();
        }

        [Test]
        [TestCase(null)]
        public async Task GetPaginatedDbLogsAsync_NullRequest_ThrowsArgumentNullException(SearchRequest request)
        {
            Should.Throw<ArgumentNullException>(() =>
            _dbLogQueryService.GetPaginatedDbLogsAsync(request));
        }

        [Test]
        public async Task GetPaginatedDbLogsAsync_ValidRequest_ReturnsPaginatedResult()
        {
            var searchRequest = new SearchRequest
            {
                PageIndex = 1,
                PageSize = 10,
                Filters = new List<FilterColumn>
                {
                    new FilterColumn
                    {
                        FilterBy = "",
                        Operator = OperatorType.Equals,
                        Value = "string",
                        IsGenericValue = true
                    }
                },
                SortOrders = new List<SortOrder>
                {
                    new SortOrder
                    {
                        SortBy = "",
                        Order = SortOrderType.Ascending
                    }
                }
            };

            var paginatedDbLogs = new Paginate<DbLog>
            {
                Index = 1,
                Size = 10,
                TotalFiltered = 402,
                Total = 402,
                Pages = 41,
                From = 1,
                Items = new List<DbLog>
                {
                    new DbLog
                    {
                        Id = 1,
                        Message = "Application starting up!",
                        Level = "Information",
                        Timestamp = DateTime.UtcNow,
                        Exception = null,
                        LogEvent = "{\"Timestamp\":\"2024-02-07T23:44:06.6693866\",\"Level\":\"Information\",\"Message\":\"Application starting up!\",\"Properties\":{\"MachineName\":\"EMRUL\"}}",
                        TraceId = null,
                        SpanId = null
                    }
                }
            };

            _applicationUnitOfWorkMock
              .Setup(x => x.LogRepository)
              .Returns(_dbLogRepositoryMock.Object);

            _dbLogRepositoryMock
                .Setup(x => x.GetPaginatedDbLogsAsync(searchRequest))
                .ReturnsAsync(paginatedDbLogs);

            var result = await _dbLogQueryService.GetPaginatedDbLogsAsync(searchRequest);

            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Index.ShouldBe(searchRequest.PageIndex),
                () => result.Size.ShouldBe(searchRequest.PageSize),
                () => result.Items.ShouldBeSameAs(paginatedDbLogs.Items),
                () => result.Total.ShouldBe(paginatedDbLogs.Total),
                () => result.TotalFiltered.ShouldBe(paginatedDbLogs.TotalFiltered),
                () => result.Pages.ShouldBe(paginatedDbLogs.Pages),
                () => result.Size.ShouldBe(paginatedDbLogs.Size)
            );
        }
    }
}
