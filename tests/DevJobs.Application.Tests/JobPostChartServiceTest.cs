using Autofac.Extras.Moq;
using DevJobs.Application.Features.JobPostAnalyzing.DTOs;
using DevJobs.Application.Features.JobPostAnalyzing.Services;
using DevJobs.Domain.Entities;
using DevJobs.Domain.Repositories;
using Moq;

namespace DevJobs.Application.Tests
{
    public class JobPostChartServiceTest
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _applicationUnitOfWork;
        private Mock<IJobPostRepository> _jobPostRepository;
        private JobPostCountService _chartService;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _applicationUnitOfWork = _mock.Mock<IApplicationUnitOfWork>();
            _jobPostRepository = _mock.Mock<IJobPostRepository>();
            _chartService = _mock.Create<JobPostCountService>();
        }

        [TearDown]
        public void Teardown()
        {
            _applicationUnitOfWork.Reset();
        }

        [Test]
        public void GetJobPostCountByDay_JobPostExist_ReturnListOfJobPostChartDTO()
        {
            // Arrange
            List<JobPost> jobPosts = new List<JobPost>
            {
                new JobPost
                {
                    Id = Guid.NewGuid(),
                    OrganizationID = Guid.NewGuid(),
                    JobTitle = "Software Developer",
                    NumberOfVacancies = 3,
                    PublishedOn = DateTime.ParseExact("04-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    JobNature = "Full-time",
                    ExperienceMin = 2,
                    ExperienceMax = 5,
                    Gender = GenderOption.Male,
                    AgeMin = 25,
                    AgeMax = 35,
                    JobLocation = "Dhaka",
                    SalaryMin = 50000,
                    SalaryMax = 70000,
                    JobDescription = "Develop and maintain software applications.",
                    EducationalRequirements = "Bachelor's degree in Computer Science or related field.",
                    ExperienceRequirements = "At least 2 years of professional experience in software development.",
                    AdditionalJobRequirements = "Strong problem-solving skills and ability to work in a team.",
                    OtherBenefits = "Health insurance, provident fund, performance bonus.",
                    Source = "Company Website",
                    CreatedOn = DateTime.Now,
                },

                new JobPost
                {
                    Id = Guid.NewGuid(),
                    OrganizationID = Guid.NewGuid(),
                    JobTitle = "Graphic Designer",
                    NumberOfVacancies = 2,
                    PublishedOn = DateTime.ParseExact("04-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    JobNature = "Part-time",
                    ExperienceMin = 1,
                    ExperienceMax = 3,
                    Gender = GenderOption.Female,
                    AgeMin = 22,
                    AgeMax = 30,
                    JobLocation = "Chittagong",
                    SalaryMin = 30000,
                    SalaryMax = 45000,
                    JobDescription = "Create visual concepts and designs.",
                    EducationalRequirements = "Diploma in Graphic Design or related field.",
                    ExperienceRequirements = "At least 1 year of experience in graphic design.",
                    AdditionalJobRequirements = "Proficiency in design software.",
                    OtherBenefits = "Flexible hours, work from home options.",
                    Source = "Online Job Portal",
                    CreatedOn = DateTime.Now.AddDays(-15),
                },

                new JobPost
                {
                    Id = Guid.NewGuid(),
                    OrganizationID = Guid.NewGuid(),
                    JobTitle = "Marketing Manager",
                    NumberOfVacancies = 1,
                    PublishedOn = DateTime.ParseExact("03-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    JobNature = "Remote",
                    ExperienceMin = 5,
                    ExperienceMax = 10,
                    Gender = GenderOption.Male,
                    AgeMin = 30,
                    AgeMax = 45,
                    JobLocation = "Sylhet",
                    SalaryMin = 80000,
                    SalaryMax = 120000,
                    JobDescription = "Lead marketing campaigns and strategies.",
                    EducationalRequirements = "MBA or equivalent in Marketing.",
                    ExperienceRequirements = "Minimum 5 years of experience in a managerial role.",
                    AdditionalJobRequirements = "Excellent communication and leadership skills.",
                    OtherBenefits = "Company car, travel opportunities, incentive bonuses.",
                    Source = "LinkedIn",
                    CreatedOn = DateTime.Now.AddDays(-30),
                },

                new JobPost
                {
                    Id = Guid.NewGuid(),
                    OrganizationID = Guid.NewGuid(),
                    JobTitle = "Human Resources Coordinator",
                    NumberOfVacancies = 2,
                    PublishedOn = DateTime.ParseExact("02-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    JobNature = "Contract",
                    ExperienceMin = 3,
                    ExperienceMax = 2,
                    Gender = GenderOption.Male,
                    AgeMin = 18,
                    AgeMax = 35,
                    JobLocation = "Rajshahi",
                    SalaryMin = 40000,
                    SalaryMax = 60000,
                    JobDescription = "Manage recruitment and employee relations.",
                    EducationalRequirements = "Bachelor's in Human Resources or related field.",
                    ExperienceRequirements = "Proven experience in HR operations and recruitment.",
                    AdditionalJobRequirements = "Knowledge of labor laws and HR best practices.",
                    OtherBenefits = "Work-life balance, wellness programs.",
                    Source = "Company Careers Page",
                    CreatedOn = DateTime.Now.AddDays(-45),
                },

                new JobPost
                {
                    Id = Guid.NewGuid(),
                    OrganizationID = Guid.NewGuid(),
                    JobTitle = "Data Analyst",
                    NumberOfVacancies = 4,
                    PublishedOn = DateTime.ParseExact("03-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    JobNature = "Internship",
                    ExperienceMin = 0,
                    ExperienceMax = 1,
                    Gender = GenderOption.Male,
                    AgeMin = 20,
                    AgeMax = 25,
                    JobLocation = "Khulna",
                    SalaryMin = 20000,
                    SalaryMax = 30000,
                    JobDescription = "Analyze data trends and provide insights.",
                    EducationalRequirements = "Currently pursuing or recently graduated with a degree in Data Science.",
                    ExperienceRequirements = "Familiarity with data analysis tools and methodologies.",
                    AdditionalJobRequirements = "Strong analytical and critical thinking skills.",
                    OtherBenefits = "Mentorship, networking opportunities, potential for full-time offer.",
                    Source = "University Career Services",
                    CreatedOn = DateTime.Now.AddDays(-60),
                }
            };

            List<JobPostCountDTO> jobPostDTOs = new List<JobPostCountDTO>()
            {
                new JobPostCountDTO
                {
                    Date = "31-12-2023",
                    JobCount = 2
                },
                new JobPostCountDTO
                {
                    Date = "01-01-2024",
                    JobCount = 2
                },
                new JobPostCountDTO
                {
                    Date = "02-01-2024",
                    JobCount = 1
                }
            };

            _applicationUnitOfWork.Setup(x => x.JobPostRepository).Returns(_jobPostRepository.Object);
            
            _jobPostRepository.Setup(x=>x.GetAll()).Returns(jobPosts);

            // Act
            List<JobPostCountDTO> result = (List<JobPostCountDTO>)_chartService.GetJobPostCountByDays(3);

            // Assert
            Assert.IsNotNull(result);

            Assert.That(result.Count, Is.EqualTo(3));
        }
    }
}
