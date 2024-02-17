using Autofac.Extras.Moq;
using DevJobs.Application.Features.JobPostVisualizing.Services;
using DevJobs.Domain.Entities;
using DevJobs.Domain.Repositories;
using DevSkill.Extensions.Paginate;
using DevSkill.Extensions.Queryable;
using Moq;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace DevJobs.Application.Tests;

[ExcludeFromCodeCoverage]
public class JobPostQueryServiceTests
{
    private AutoMock _mock;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<IJobPostRepository> _jobpostRepositoryMock;
    private IJobPostQueryService _jobpostQueryService;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _mock = AutoMock.GetLoose();
    }

    [SetUp]
    public void Setup()
    {
        _applicationUnitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
        _jobpostRepositoryMock = _mock.Mock<IJobPostRepository>();
        _jobpostQueryService = _mock.Create<JobPostQueryService>();
    }

    [TearDown]
    public void TearDown()
    {
        _applicationUnitOfWorkMock.Reset();
        _jobpostRepositoryMock.Reset();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _mock?.Dispose();
    }

    [Test]
    public async Task GetJobPostAsync_ValidId_ReturnsJobPost()
    {
        // Arrange
        var jobId = Guid.Parse("3721a3c1-2267-48fc-813a-1a83b4cd6375");

        JobPost expectedJobPost = new JobPost
        {
            Id = jobId,
            OrganizationID = Guid.Parse("d2b5e7a8-19bd-40f9-f47c-08dc19fe57b9"),
            Company = new Organization
            {
                Id = Guid.Parse("d2b5e7a8-19bd-40f9-f47c-08dc19fe57b9"),
                Name = "Keya Group",
                Website = null,
                Address = null,
                BusinessType = null
            },
            JobTitle = "Web Developer (Business Development)",
            NumberOfVacancies = 1,
            PublishedOn = DateTime.Parse("2024-01-20T21:42:17.3906557"),
            JobNature = "Full-time",
            ExperienceMin = 2,
            ExperienceMax = 5,
            Gender = null,
            AgeMin = 27,
            AgeMax = 35,
            JobLocation = "gazipur",
            SalaryMin = null,
            SalaryMax = null,
            JobDescription = "We are searching Full Stack Developer who is capable to develop and design web based applications.",
            EducationalRequirements = "Bachelor of Science (BSc) in CSE,",
            ExperienceRequirements = "2 to 5 years",
            AdditionalJobRequirements = "Age 27 to 35 years",
            OtherBenefits = "Festival Bonus= 2\r\nAs per company law",
            Source = "www.bdjobs.com",
            CreatedOn = DateTime.Parse("2024-01-20T23:01:20.5204831"),
            Analysis = new List<JobAnalysis>
            {
                new JobAnalysis
                {
                    Id = Guid.Parse("3519093e-a852-44de-85fe-08dc19fe57b7"),
                    JobPostID = jobId,
                    JobTrack = Track.DotNet,
                    Experience = ExperienceLevel.MidLevel,
                    Technologies = new List<JobTechnology>
                    {
                        new() { Id = Guid.Parse("5c144544-c8aa-40f3-adda-08dc19fe5f50"), Name = "Laravel" },
                        new() { Id = Guid.Parse("575be043-fcf1-497f-addb-08dc19fe5f50"), Name = "PHP" },
                        new() { Id = Guid.Parse("d66a71b5-2fe6-4cce-addc-08dc19fe5f50"), Name = "Java" },
                        new() { Id = Guid.Parse("bb9d4111-5b9c-4aca-addd-08dc19fe5f50"), Name = "WordPress" },
                    }
                }
            }
        };

        _applicationUnitOfWorkMock
            .Setup(x => x.JobPostRepository)
            .Returns(_jobpostRepositoryMock.Object);

        _jobpostRepositoryMock.
            Setup(x => x.GetJobPostAsync(jobId)).
            ReturnsAsync(expectedJobPost);

        // Act
        var result = await _jobpostQueryService.GetJobPostAsync(jobId);

        // Assert
        this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(jobId)
            );
    }

    [Test]
    public async Task GetJobPostAsync_NonExistentId_ReturnsNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        _applicationUnitOfWorkMock
            .Setup(x => x.JobPostRepository)
            .Returns(_jobpostRepositoryMock.Object);

        _jobpostRepositoryMock.
            Setup(x => x.GetJobPostAsync(nonExistentId)).
            ReturnsAsync((JobPost)null);

        // Act
        var result = await _jobpostQueryService.GetJobPostAsync(nonExistentId);

        // Assert
        result.ShouldBeNull();
    }

    [Test]
    [TestCase(null)]
    public async Task GetPaginatedJobPostsAsync_NullRequest_ThrowsArgumentNullException(SearchRequest request)
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
        _jobpostQueryService.GetPaginatedJobPostsAsync(request));
    }

    [Test]
    public async Task GetPaginatedJobPostsAsync_ValidRequest_ReturnsPaginatedResult()
    {
        // Arrange
        var searchRequest = new SearchRequest
        {
            PageIndex = 1,
            PageSize = 2,
            Filters =
            [
                new FilterColumn()
                {
                    FilterBy = "",
                    Operator = OperatorType.Equals,
                    Value = "string",
                    IsGenericValue = true
                }
            ],
            SortOrders =
            [
                new SortOrder()
                {
                    SortBy = "JobTitle",
                    Order = SortOrderType.Ascending
                }
            ]
        };

        var paginatedJobPosts = new Paginate<JobPost>
        {
            Index = 1,
            Size = 2,
            TotalFiltered = 4172,
            Total = 4172,
            Pages = 2086,
            From = 1,
            Items =
            [
                new JobPost
                {
                    Id = new Guid("e7255505-8b8e-47f1-a303-dda92525e598"),
                    OrganizationID = new Guid("afe3fb96-f3d7-4604-ee24-08dc19fe57b9"),
                    Company = null,
                    JobTitle = "২০২৫এ অফিসার ক্যাডেট ব্যাচ",
                    NumberOfVacancies = 1,
                    JobNature = "Full-time",
                    ExperienceMin = 0,
                    ExperienceMax = null,
                    Gender = null,
                    AgeMin = null,
                    AgeMax = null,
                    JobLocation = "বাংলাদেশের যেকোনো স্থানে",
                    SalaryMin = null,
                    SalaryMax = null,
                    JobDescription = "বিস্তারিত জানতে ও আবেদন করতে এই লিংক ভিজিট করুন= https://hotjobs.bdjobs.com",
                    EducationalRequirements = null,
                    ExperienceRequirements = null,
                    AdditionalJobRequirements = null,
                    OtherBenefits = null,
                    Source = "www.bdjobs.com",
                    CreatedOn = DateTime.Parse("2024-01-20T21:54:03.2596528"),
                    Analysis = null
                },
                new JobPost()
                {
                    Id = new Guid("60a6fe83-7e1e-4ae9-9021-67200b9b0169"),
                    OrganizationID = new Guid("6f417b78-3e0d-4435-ecf4-08dc19fe57b9"),
                    Company = null,
                    JobTitle = "3D Animation Specialist",
                    NumberOfVacancies = 1,
                    PublishedOn = DateTime.Parse("2024-01-17T00:00:00"),
                    JobNature = "Full-time",
                    ExperienceMin = 2,
                    ExperienceMax = 3,
                    Gender = null,
                    AgeMin = null,
                    AgeMax = null,
                    JobLocation = "dhaka (banani)",
                    SalaryMin = null,
                    SalaryMax = null,
                    JobDescription = "Office Time: 09:30am-06:30pm (Six Days)",
                    EducationalRequirements = "Bachelor's degree in Animation, Visual Effects",
                    ExperienceRequirements = "2 to 3 years",
                    AdditionalJobRequirements = null,
                    OtherBenefits = "Salary: Negotiable (Based on experience and qualification).",
                    Source = "www.bdjobs.com",
                    CreatedOn = DateTime.Parse("2024-01-20T21:42:17.3906557"),
                    Analysis = null
                }
            ],
        };

        _applicationUnitOfWorkMock
          .Setup(x => x.JobPostRepository)
          .Returns(_jobpostRepositoryMock.Object);

        _jobpostRepositoryMock
            .Setup(x => x.GetPaginatedJobPostsAsync(searchRequest))
            .ReturnsAsync(paginatedJobPosts);

        // Act
        var result = await _jobpostQueryService.GetPaginatedJobPostsAsync(searchRequest);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.Index.ShouldBe(searchRequest.PageIndex),
            () => result.Size.ShouldBe(searchRequest.PageSize),
            () => result.Items.ShouldBeSameAs(paginatedJobPosts.Items),
            () => result.Total.ShouldBe(paginatedJobPosts.Total),
            () => result.TotalFiltered.ShouldBe(paginatedJobPosts.TotalFiltered),
            () => result.Pages.ShouldBe(paginatedJobPosts.Pages),
            () => result.Size.ShouldBe(paginatedJobPosts.Size)
        );
    }
}