using Autofac;
using DevJobs.Application.DTOs;
using DevJobs.Application.Features.Services;
using DevJobs.Infrastructure.Features.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace DevJobs.Api.Request
{
    public class ForgotPasswordRequestHandler
    {
        public string Email { get; set; }
        public string Origin { get; set; }

        private IForgotPasswordService _forgotPasswordService;
        private IEmailMessageService _emailMessageService;

        public ForgotPasswordRequestHandler() { }

        public ForgotPasswordRequestHandler(
            IForgotPasswordService forgotPasswordService,
            IEmailMessageService emailMessageService)
        {
            _forgotPasswordService = forgotPasswordService;
            _emailMessageService = emailMessageService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _forgotPasswordService = scope.Resolve<IForgotPasswordService>();
            _emailMessageService = scope.Resolve<IEmailMessageService>();
        }

        public async Task<object> GeneratePasswordResetTokenAndSendEmail()
        {
            var forgotPasswordDTO = await _forgotPasswordService.GeneratePasswordResetToken(Email, Origin);
            return await _emailMessageService.SendResetPasswordEmailAsync(
                forgotPasswordDTO.IsSuccess, forgotPasswordDTO.FullName, forgotPasswordDTO.Email, forgotPasswordDTO.CallbackUrl);
        }
    }
}
