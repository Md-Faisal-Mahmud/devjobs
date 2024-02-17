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
    [SwaggerTag("Service Status Checker")]
    public class WorkerServiceStatusCheckController : ControllerBase
    {
        private readonly ILifetimeScope _scope;

        public WorkerServiceStatusCheckController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        /// <remarks>
        /// 
        /// Sample response:
        ///    
        ///    "running"
        ///    
        ///    "stopped" 
        /// </remarks>
        [SwaggerOperation(Summary = "Check Worker Service running status")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request given successfully", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request failed", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized: User doesn't have required permission", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        [HttpGet, Authorize(Policy = "ServiceStatusView")]
        public IActionResult CheckServiceStatus()
        {
            var request = _scope.Resolve<WorkerServiceStatusRequestHandler>();
            request.ResolveDependency(_scope);
            bool isServiceRunning = request.CheckServiceStatus();
            if (isServiceRunning)
            {
                return Ok(new { status = "running" });
            }
            else
            {
                return Ok(new { status = "stopped" });
            }
        }
    }
}
