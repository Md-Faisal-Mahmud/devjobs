using Autofac;
using DevJobs.Api.Request;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using DevSkill.Extensions.Queryable;
using Microsoft.AspNetCore.Authorization;
using DevJobs.Infrastructure.Utilities;
using DevJobs.Infrastructure.Securities.DbLogViewRequirement;

namespace DevJobs.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [EnableCors("AllowWebApp")]
    [SwaggerTag("Manage Database Logs")]
    public class DbLogController : ControllerBase
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<DbLogController> _logger;

        public DbLogController(ILifetimeScope lifetimeScope, ILogger<DbLogController> logger)
        {
            _scope = lifetimeScope;
            _logger = logger;
        }

        /// <remarks>
        /// Sample request:
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
        ///                 "sortBy": "",
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
        ///         "items": [
        ///             {
        ///                 "id": 1,
        ///                 "message": "Application starting up!",
        ///                 "level": "Information",
        ///                 "timestamp": "2024-02-07T23:44:06.6693866",
        ///                 "exception": null,
        ///                 "logEvent": "{\"Timestamp\":\"2024-02-07T23:44:06.6693866\",\"Level\":\"Information\",\"Message\":\"Application starting up!\",\"Properties\":{\"MachineName\":\"EMRUL\"}}",
        ///                 "traceId": null,
        ///                 "spanId": null
        ///             }
        ///         ]
        ///     }
        /// </remarks>
        [SwaggerOperation(Summary = "Get Data collector log list with pagination")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request validation failed", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized: User lacks required permission", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        [HttpPost, Authorize(Policy = "LogListViewRequirementPolicy")]
        public async Task<IActionResult> Post([FromBody] SearchRequest request)
        {
            try
            {
                var logListRequestHandler = _scope.Resolve<DbLogListRequestHandler>();

                logListRequestHandler.ResolveDependency(_scope);

                var paginatedLogs = await logListRequestHandler.GetPaginatedDbLogsAsync(request);

                return Ok(paginatedLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't get Logs.");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        /// <remarks>
        /// Sample request:
        /// 
        ///     1
        ///
        /// Sample response:
        /// 
        ///     {
        ///         "Id": "1",
        ///         "Message": "Application starting up!",
        ///         "Level": "Information",
        ///         "Timestamp": "2024-02-07T23:44:06.6693866",
        ///         "Exception": null,
        ///         "LogEvent": "{\"Timestamp\":\"2024-02-07T23:44:06.6693866\",\"Level\":\"Information\",\"Message\":\"Application starting up!\",\"Properties\":{\"MachineName\":\"EMRUL\"}}",
        ///         "TraceId": null,
        ///         "SpanId": null
        ///     }
        /// </remarks>
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get Log by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request validation failed", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized: User lacks required permission", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        [HttpGet("{id}"), Authorize(Policy = "LogListViewRequirementPolicy")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var request = _scope.Resolve<DbLogRequestHandler>();
                var log = await request.GetLogAsync(id);
                var serializedData = await JsonResultSerializer.SerializeAsync(log);

                return Content(serializedData, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't get logs.");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        /// <remarks>
        /// This endpoint allows deleting logs from the database based on provided id.
        /// </remarks>
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Delete Log by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Log deleted successfully", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request validation failed", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized: User lacks required permission", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteLogByIdRequirementPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dbLogRequestHandler = _scope.Resolve<DbLogRequestHandler>();
                dbLogRequestHandler.ResolvedDependency(_scope);

                await dbLogRequestHandler.DeleteDbLogAsync(id);

                return Ok(new { message = "Log deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't delete log.");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }        
    }    
}
