using Autofac;
using DevJobs.Api.Request;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using DevSkill.Extensions.Queryable;
using Microsoft.AspNetCore.Authorization;

namespace DevJobs.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [EnableCors("AllowWebApp")]
    [SwaggerTag("Manage Organization")]
    public class OrganizationsController : ControllerBase
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<OrganizationsController> _logger;
        public OrganizationsController(ILifetimeScope scope, ILogger<OrganizationsController> logger)
        {
            _scope = scope;
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
        ///                 "filterBy": "name",
        ///                 "operator": 1,
        ///                 "value": "string",
        ///                 "isGenericValue": true
        ///             }
        ///         ],
        ///         "sortOrders": [
        ///             {
        ///                 "sortBy": "name",
        ///                 "order": 1
        ///             }
        ///         ]
        ///     }
        /// Sample response:
        /// 
        ///     {
        ///         "from": 1,
        ///         "index": 1,
        ///         "size": 2,
        ///         "totalFiltered": 62,
        ///         "total": 62,
        ///         "pages": 31,
        ///         "items:
        ///     
        ///             [
        ///                 {
        ///                     "Name" : "Mondol Group",
        ///                     "Website" : "www.mondol.net",
        ///                     "Address" : "La-Meridian",
        ///                     "BusinessType" : "Textile",
        ///                 },
        ///                 {
        ///                     "Name" : "Dhaka Group",
        ///                     "Website" : "www.mondol.net",
        ///                     "Address" : "La-Meridian",
        ///                     "BusinessType" : "Textile",
        ///                 }
        ///             ]
        ///     }
        /// </remarks>

        [SwaggerOperation(Summary = "Get paginate Organizations list with filter and sort option")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request validation failed", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized: User lacks required permission", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        [Produces("application/json")]

        [HttpPost, Authorize(Policy = "JobViewRequirementPolicy")]
        public async Task<IActionResult> Post([FromBody] SearchRequest request)
        {            
            try
            {
                var requestHandler = _scope.Resolve<OrganizationListRequestHandler>();
                var data = await requestHandler.GetOrganizationListAsync(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't get Organization");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
    }
}
