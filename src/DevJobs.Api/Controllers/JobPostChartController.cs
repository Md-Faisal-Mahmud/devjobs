using Autofac;
using DevJobs.Api.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DevJobs.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    [EnableCors("AllowWebApp")]
    [SwaggerTag("Job Post Chart Information")]
    public class JobPostChartController : ControllerBase
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILogger<JobPostChartController> _logger;

        public JobPostChartController(ILifetimeScope scope, ILogger<JobPostChartController> logger) 
        {
            _lifetimeScope = scope;
            _logger = logger;
        }

        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "days": 3
        ///     }
        /// 
        /// Sample response:
        /// 
        ///      [
        ///        {
        ///          "day": "05-12-2023",
        ///          "count": 9
        ///        },
        ///        {
        ///          "day": "06-12-2023",
        ///          "count": 12
        ///        },
        ///        {
        ///          "day": "07-12-2023",
        ///          "count": 11
        ///        }, 
        ///      ]
        /// </remarks>
        [HttpGet()]
        [EnableCors("AllowWebApp")]
        [SwaggerOperation(Summary = "Get the number of Job Post according to date, Default is last 30 days")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Something went wrong", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request failed", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        [Produces("application/json")]
        public object GetCountByDate([FromQuery]JobPostCountDataRequestHandler request)
        {
            try
            {
                request.ResolveDependency(_lifetimeScope);
                var data = request.GetJobPostCountByDate(request.Days);
                return Ok(data);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
    }
}
