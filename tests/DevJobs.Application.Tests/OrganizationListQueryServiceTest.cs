using Autofac.Extras.Moq;
using DevJobs.Domain.Entities;
using DevJobs.Domain.Repositories;
using DevJobs.Infrastructure.Repositories;
using DevJobs.Infrastructure;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Paginate.Extensions;
using DevSkill.Extensions.Queryable;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using DevJobs.Application.Features.OrganizationList;
using Shouldly;
using System.Linq.Expressions;

namespace DevJobs.Application.Tests
{
    public class OrganizationListQueryServiceTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<IJobPostRepository> _jobPostRepository;
        private OrganizationListQueryService _organizationListQueryService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
            _jobPostRepository = _mock.Mock<IJobPostRepository>();
            _organizationListQueryService = _mock.Create<OrganizationListQueryService>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public async Task GetOrganizationList_AsperRequest()
        {           
            // Arrange
            var jobPosts = new List<JobPost>
            {
               // Add some job posts with companies for testing
               new JobPost { Id = Guid.NewGuid(), Company = new Organization(){ Id = Guid.NewGuid()} },
               new JobPost { Id = Guid.NewGuid(), Company = new Organization(){ Id = Guid.NewGuid()} },
               new JobPost { Id = Guid.NewGuid(), Company = new Organization(){ Id = Guid.NewGuid()} },
            };

            _jobPostRepository.Setup(x => x.Get(
                It.IsAny<Expression<Func<JobPost, bool>>>(), It.IsAny<Func<IQueryable<JobPost>, IIncludableQueryable<JobPost, object>>>()))
                .Returns(jobPosts);

            _unitOfWorkMock.SetupGet(uow => uow.JobPostRepository).Returns(_jobPostRepository.Object);

            var searchRequest = new SearchRequest
            {
                PageIndex = 1,
                PageSize = 2,
            };

            // Act
            var result = await _organizationListQueryService.GetOrganizationList(searchRequest);

            // Assert
            this.ShouldSatisfyAllConditions(
              () => result.ShouldNotBeNull(),
              () => result.TotalFiltered.ShouldBe(3),
              () => result.Index.ShouldBe(searchRequest.PageIndex),
              () => result.Size.ShouldBe(searchRequest.PageSize)
              );
        }

        [Test]
        [TestCase(null)]
        public void Organization_Null_ThrowsException(SearchRequest request)
        {
            // Arrange, Act & Assert
            Should.Throw<ArgumentNullException>(() =>
                 _organizationListQueryService.GetOrganizationList(request));
        }
    }
}

