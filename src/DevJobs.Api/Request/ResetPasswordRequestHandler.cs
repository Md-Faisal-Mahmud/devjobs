using Autofac;
using DevJobs.Application.Features.Services;
using Microsoft.AspNetCore.Mvc;
namespace DevJobs.Api.Request
{
    public class ResetPasswordRequestHandler
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        private IResetPasswordService _resetPasswordService;

        public ResetPasswordRequestHandler() { }

        public ResetPasswordRequestHandler(IResetPasswordService resetPasswordService)
        {
            _resetPasswordService = resetPasswordService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _resetPasswordService = scope.Resolve<IResetPasswordService>();
        }

        public async Task<object> VerifyResetPasswordRequest()
        {
            return await _resetPasswordService.VerifyResetPasswordRequest(UserId, Code, Password, ConfirmPassword);
        }
    }
}
