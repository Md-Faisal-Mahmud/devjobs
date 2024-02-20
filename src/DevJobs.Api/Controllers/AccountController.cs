using Autofac;
using DevJobs.Api.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DevJobs.Api.Controllers
{
    [Route("v1/[controller]/[action]")]
    [ApiController]
    [EnableCors("AllowWebApp")]
    [SwaggerTag("Logs in a user")]
    public class AccountController(ILogger<AccountController> logger, ILifetimeScope scope) : ControllerBase
    {
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "email": "xyz@devjobs.com",
        ///       "password": "123456",
        ///       "rememberMe": false 
        ///     }
        /// 
        /// Sample response:
        /// 
        ///     {
        ///       "success": true,
        ///       "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6I",
        ///       "name": "Mr XYZ",
        ///       "username": "xyz@devjobs.com",
        ///       "email": "xyz@devjobs.com",
        ///       "role": XYZ,
        ///       "image": "https://devjobs.com/user.png"
        ///     }
        /// </remarks>
        [SwaggerOperation(
            Summary = "Check the user credentials and return the status code, token, and user information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Login successful", typeof(LoginRequestHandler))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request validation failed", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Login([FromBody] LoginRequestHandler loginRequestHandler)
        {
            try
            {
                loginRequestHandler.ResolveDependency(scope);
                return Ok(await loginRequestHandler.LoginUserAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Couldn't login");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "recaptchaToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6I"
        ///     }
        /// 
        /// Sample response:
        /// 
        ///     {
        ///       "success": true
        ///     }
        /// </remarks>
        [SwaggerOperation(
            Summary = "Verify reCAPTCHA token")]
        [SwaggerResponse(StatusCodes.Status200OK, "reCAPTCHA token is valid", typeof(RecaptchaRequestHandler))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request validation failed", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> RecaptchaVerify([FromBody] RecaptchaRequestHandler recaptchaRequestHandler)
        {
            try
            {
                recaptchaRequestHandler.ResolveDependency(scope);
                return Ok(await recaptchaRequestHandler.VerifyRecaptchaTokenAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Couldn't verify recaptcha");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }


        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "email": "xyz@devjobs.com",
        ///       "origin": "http://localhost:4200"
        ///     }
        /// 
        /// Sample response:
        /// 
        ///     {
        ///       "success": true
        ///     }
        /// </remarks>
        [SwaggerOperation(
            Summary = "Initiate the process for resetting the user's password")]
        [SwaggerResponse(StatusCodes.Status200OK, "Password reset initiated successfully. An email with further instructions will be sent.",
            typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input or request validation failed.", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred during the password reset process.",
            typeof(IResult))]
        [Produces("application/json")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestHandler forgotPasswordRequestHandler)
        {
            try
            {
                forgotPasswordRequestHandler.ResolveDependency(scope);
                return Ok(await forgotPasswordRequestHandler.GeneratePasswordResetTokenAndSendEmail());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Couldn't forgot password");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "userId": "66c3aef9-9444-4c71-2db4-08dc050745ff",
        ///       "code": "eyJhbGciOiJIUzI1NiIsInR5cCI6I",
        ///       "password": "123456",
        ///       "confirmPassword": "123456"
        ///     }
        /// 
        /// Sample response:
        /// 
        ///     {
        ///       "success": true
        ///     }
        /// </remarks>
        [SwaggerOperation(
            Summary = "Initiate the process for resetting the user's password")]
        [SwaggerResponse(StatusCodes.Status200OK, "Password reset initiated successfully", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request validation failed or invalid input", typeof(IResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred", typeof(IResult))]
        [Produces("application/json")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestHandler resetPasswordRequestHandler)
        {
            try
            {
                resetPasswordRequestHandler.ResolveDependency(scope);
                return Ok(await resetPasswordRequestHandler.VerifyResetPasswordRequest());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Couldn't reset password");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
    }
}
