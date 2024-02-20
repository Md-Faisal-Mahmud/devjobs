using Autofac;
using DevJobs.Api.Request;
using DevJobs.Domain.Entities;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Queryable;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using DevJobs.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace DevJobs.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
[EnableCors("AllowWebApp")]
[SwaggerTag("Visualizing JobPosts")]
public class JobPostController : ControllerBase
{
    private readonly ILifetimeScope _scope;
    private readonly ILogger<JobPostController> _logger;

    public JobPostController(ILifetimeScope scope, ILogger<JobPostController> logger)
    {
        _scope = scope;
        _logger = logger;
    }

    /// <remarks>
    ///Sample request:
    /// 
    ///     {
    ///         "pageIndex": 1,
    ///         "pageSize": 10,
    ///         "filters": [
    ///             {
    ///                 "filterBy": "",
    ///                 "operator": 1,
    ///                 "value": "string",
    ///                 "isGenericValue": true
    ///             }
    ///         ],
    ///         "sortOrders": [
    ///             {
    ///                 "sortBy": "JobTitle",
    ///                 "order": 1
    ///             }
    ///         ]
    ///     }
    /// Sample response:
    /// 
    ///     {
    ///         "from": 1,
    ///         "index": 1,
    ///         "size": 10,
    ///         "totalFiltered": 402,
    ///         "total": 402,
    ///         "pages": 41,
    ///         "items:
    ///     
    ///             [
    ///                 {
    ///                     "id": "72724289-82cb-4cbc-82ca-005ddff97097",
    ///                     "organizationID": "289923af-ec18-4b0a-a379-08dc14cd47b3",
    ///                     "jobTitle": "Senior Web Developer  (Laravel Frameworks)",
    ///                     "numberOfVacancies": 2,
    ///                     "publishedOn": "2024-01-09T00:00:00",
    ///                     "jobNature": "Full-time",
    ///                     "experienceMin": 5,
    ///                     "experienceMax": 10,
    ///                     "gender": null,
    ///                     "ageMin": 25,
    ///                     "ageMax": 41,
    ///                     "jobLocation": "dhaka",
    ///                     "salaryMin": null,
    ///                     "salaryMax": null,
    ///                     "jobDescription": "Working on PHP (Laravel &amp; CodeIgniter Frameworks).Knowledge of web development frameworks.",
    ///                     "educationalRequirements": "Bachelor of Science (BSc) in EEE, Computer Science, IT",
    ///                     "experienceRequirements": "5 to 10 years",
    ///                     "additionalJobRequirements": "Age 25 to 41 years",
    ///                     "otherBenefits": "Mobile bill\r\nSalary Review: Yearly",
    ///                     "source": "www.bdjobs.com",
    ///                     "createdOn": "2024-01-14T18:04:00.0667223"
    ///                 }
    ///             ]
    ///     }
    /// </remarks>
    [SwaggerOperation(Summary = "Get JobPosts list with pagination")]
    [SwaggerResponse(StatusCodes.Status200OK, "Request successful", typeof(IResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Request validation failed", typeof(IResult))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized: User lacks required permission", typeof(IResult))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
    [HttpPost, Authorize(Policy = "JobListViewRequirementPolicy")]
    public async Task<IActionResult> Post([FromBody] SearchRequest request)
    {
        try
        {
            var jobPostListRequestHandler = _scope.Resolve<JobPostListRequestHandler>();

            jobPostListRequestHandler.ResolveDependency(_scope);

            var paginatedJobPosts = await jobPostListRequestHandler.GetPaginatedJobPostsAsync(request);

            return Ok(paginatedJobPosts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't get JobPosts.");
            return StatusCode(500, new { message = "Internal server error occurred" });
        }
    }

    /// <remarks>
    ///Sample request:
    /// 
    ///     cf5f81c4-db31-49f2-be4c-5ce762dc6790
    ///
    /// Sample response:
    /// 
    ///     {
    ///         "Id": "cf5f81c4-db31-49f2-be4c-5ce762dc6790",
    ///         "OrganizationID": "79a38421-9133-4cbb-57d7-08dc18fb5778",
    ///         "Company":{
    ///             "Id": "79a38421-9133-4cbb-57d7-08dc18fb5778",
    ///             "Name": "Continental Technologies BD",
    ///             "Website": null,
    ///             "Address": "Khurdukhosh Parra, Mijmiji,P.S. : Siddhirgonj,Dist : Narayangonj, Bangladesh",
    ///             "BusinessType": "We focus to be a trusted partner and strategic technology advisor to our clients."
    ///          },
    ///         "JobTitle": "Head of Software Development",
    ///         "NumberOfVacancies": 1,
    ///         "PublishedOn": "2024-01-04T00:00:00",
    ///         "JobNature": "Full-time",
    ///         "ExperienceMin": 10,
    ///         "ExperienceMax": 14,
    ///         "Gender": null,
    ///         "AgeMin": null,
    ///         "AgeMax": 50,
    ///         "JobLocation": "narayanganj",
    ///         "SalaryMin": 80000,
    ///         "SalaryMax": 120000,
    ///         "JobDescription": "We are looking for a talented and experienced Technical project manager to join our team.Evaluate project performance and outcomes",
    ///         "EducationalRequirements": "Masters in Computer Application (MCA) in Computer Science",
    ///         "ExperienceRequirements": "10 to 14 years: Software Company",
    ///         "AdditionalJobRequirements": "Age at most 50 years",
    ///         "OtherBenefits": "Salary Review: Yearly\r\nFestival Bonus: 2",
    ///         "Source": "www.bdjobs.com",
    ///         "CreatedOn": "2024-01-19T16:07:56.1034363",
    ///         "Analysis":[
    ///             {
    ///                 "Id": "64bb50a8-1c21-4779-80f6-08dc18fb5775",
    ///                 "Post": null,
    ///                 "JobPostID": "cf5f81c4-db31-49f2-be4c-5ce762dc6790",
    ///                 "JobTrack": 11,
    ///                 "Experience": 1,
    ///                 "Technologies":[
    ///                     {
    ///                             "Id": "b4f100fc-9c8e-48d2-96fd-08dc18fb5777",
    ///                             "Name": "perl"
    ///                     }
    ///                 ]
    ///             }
    ///         ]
    ///     }
    /// </remarks>
    [Produces("application/json")]
    [SwaggerOperation(Summary = "Get JobPost by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Request successful", typeof(IResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Request validation failed", typeof(IResult))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized: User lacks required permission", typeof(IResult))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
    [HttpGet("{id}"), Authorize(Policy = "JobDetailsViewRequirementPolicy")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var request = _scope.Resolve<JobPostGetRequestHandler>();
            var post = await request.GetJobPostAsync(id);
            var serializedData = await JsonResultSerializer.SerializeAsync(post);

            return Content(serializedData, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't get JobPosts.");
            return StatusCode(500, new { message = "Internal server error occurred" });
        }
    }
}