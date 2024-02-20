using Autofac;
using DevJobs.Api.Request.UserManagement;
using DevJobs.Application.Features.Membership.DTOs;
using DevSkill.Extensions.Queryable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DevJobs.Api.Controllers
{
    [Route("v1/[controller]/[action]")]
    [ApiController]
    [EnableCors("AllowWebApp")]
    [SwaggerTag("Manage User")]
    [Authorize]
    public class UserManagementController : ControllerBase
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(ILifetimeScope scope, ILogger<UserManagementController> logger)
        {
            _lifetimeScope = scope;
            _logger = logger;
        }

        /// <remarks>
        /// Sample request:
        /// 
        ///       {
        ///         "firstName": "Abdur",
        ///         "lastName": "Rahim",
        ///         "username": "rahim@devjobs.com",
        ///         "email": "rahim@devjobs.com",
        ///         "phoneNumber": "01794914570",
        ///         "userEmail": "rahim@devjobs.com",
        ///         "userPassword": "123456"
        ///       }
        /// 
        /// Sample response:
        /// 
        ///       {
        ///         "succeeded": false,
        ///         "errors": 
        ///         [
        ///            {
        ///              "code": "DuplicateUserName",
        ///              "description": "Username 'rahim@devjobs.com' is already taken."
        ///            }
        ///         ]
        ///       }
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "UserCreatePolicy")]
        [SwaggerOperation(Summary = "Create a User")]
        [SwaggerResponse(StatusCodes.Status200OK, "Succeeded", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Something Went Wrong", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        public async Task<object> CreateUser(UserCreateDTO user)
        {

            try
            {
                var request = _lifetimeScope.Resolve<UserCreateRequestHandler>();
                return await request.CreateUser(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        /// <remarks>
        /// Sample request
        /// 
        ///       {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "firstName": "John",
        ///         "lastName": "Doe",
        ///         "userName": "johndoe",
        ///         "phoneNumber": "1234567890",
        ///         "phoneNumberConfirmed": true,
        ///         "email": "johndoe@example.com",
        ///         "emailConfirmed": true,
        ///         "twoFactorEnabled": true,
        ///         "lockoutEnd": "2024-01-25T14:33:48.766Z",
        ///         "lockoutEnabled": true,
        ///         "accessFailedCount": 0
        ///       }
        ///       
        /// Sample response:
        /// 
        ///       {
        ///         "succeeded": false,
        ///         "errors": 
        ///         [
        ///            {
        ///              "code": "DuplicateUserName",
        ///              "description": "Username 'johndoe@devjobs.com' is already taken."
        ///            }
        ///         ]
        ///       }
        /// </remarks>

        [HttpPost]
        [Authorize(Policy = "UserUpdatePolicy")]
        [SwaggerOperation(Summary = "Update a User")]
        [SwaggerResponse(StatusCodes.Status200OK, "Succeeded", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Something Went Wrong", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        public async Task<object> UpdateUser(UserDetailsDTO user)
        {
            try
            {
                var model = _lifetimeScope.Resolve<UserUpdateRequestHandler>();
                return await model.UpdateUser(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        /// <summary>
        /// Retrieves a  user with by userId.
        /// </summary>
        /// <returns>
        /// Returns a JSON object containing user information.
        /// </returns>
        /// <remarks>
        /// 
        ///  Sample request body:
        /// 
        ///       {
        ///        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///       }
        ///
        /// Sample response:
        /// 
        ///       {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "firstName": "John",
        ///         "lastName": "Doe",
        ///         "userName": "johndoe",
        ///         "phoneNumber": "1234567890",
        ///         "phoneNumberConfirmed": true,
        ///         "email": "johndoe@example.com",
        ///         "emailConfirmed": true,
        ///         "twoFactorEnabled": true,
        ///         "lockoutEnd": "2024-01-25T14:33:48.766Z",
        ///         "lockoutEnabled": true,
        ///         "accessFailedCount": 0
        ///       }
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "UserViewPolicy")]
        [SwaggerResponse(204, Description ="No User Found")]
        [SwaggerResponse(200, Type = typeof(UserDetailsDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Something Went Wrong", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        public async Task<object> GetUserById([FromBody] UserGetByIdRequestHandler request)
        {
            try
            {
                request.ResolveDependency(_lifetimeScope);
                return await request.GetUserByIdAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        /// <summary>
        /// Retrieves a list of users with pagination information.
        /// </summary>
        /// <returns>
        /// Returns a JSON object containing user information along with pagination details.
        /// </returns>
        /// <remarks>
        /// 
        ///  Sample request body:
        /// 
        ///       {
        ///         "pageIndex": 1,
        ///         "pageSize": 10,
        ///         "filters": 
        ///         [
        ///          {
        ///            "filterBy": "",
        ///            "operator": 1,
        ///            "value": "",
        ///            "isGenericValue": true
        ///          }
        ///         ],
        ///         "sortOrders": 
        ///         [
        ///          {
        ///            "sortBy": "firstname",
        ///            "order": 1
        ///           }
        ///          ]
        ///        }
        ///
        /// Sample response:
        /// 
        ///       {
        ///         "index": 1,
        ///         "size": 10,
        ///         "totalFiltered": 11,
        ///         "total": 11,
        ///         "pages": 2,
        ///         "from": 1,
        ///         "items": 
        ///         [
        ///           {
        ///              "userId": "396b2690-f0a3-4b56-aac8-08dc118b37b6",
        ///              "firstName": "Hasan",
        ///              "lastName": "Mahmud",
        ///              "userName": "hasan@devjobs.com",
        ///              "phoneNumber": "01794914570",
        ///              "userEmail": "hasan@devjobs.com"
        ///            }
        ///         ],
        ///         "hasPrevious": false,
        ///         "hasNext": true
        ///       }
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "UserViewPolicy")]
        [SwaggerResponse(StatusCodes.Status200OK, "Succeeded", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Something Went Wrong", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        public async Task<object> GetUsersAsync(SearchRequest searchRequest)
        {
            try
            {
               var request = _lifetimeScope.Resolve<UserGetRequestHandler>();
                return await request.GetUsersAsync(searchRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        /// <summary>
        /// Delete a user with userId.
        /// </summary>
        /// <returns>
        /// Returns a JSON object success or error message.
        /// </returns>
        /// <remarks>
        /// 
        ///  Sample request body:
        /// 
        ///       {
        ///        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///       }
        ///
        /// Sample response:
        /// 
        ///       {
        ///         "succeeded": true,
        ///         "errors": []
        ///        }
        /// </remarks>

        [HttpPost]
        [Authorize(Policy = "UserDeletePolicy")]
        [SwaggerResponse(StatusCodes.Status200OK, "Succeeded", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Not Found", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Something Went Wrong", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        public async Task<object> DeleteUserById([FromBody]UserDeleteByIdRequestHandler request)
        {
            try
            {
                request.ResolveDependency(_lifetimeScope);
                return await request.DeleteUserById();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }     
    }
}
