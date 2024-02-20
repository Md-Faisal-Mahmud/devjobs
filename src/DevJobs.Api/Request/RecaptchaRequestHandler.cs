using Autofac;
using DevJobs.Application.Features.Services;
using DevJobs.Infrastructure.Features.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevJobs.Api.Request
{
    public class RecaptchaRequestHandler
    {
        public string RecaptchaToken { get; set; }

        private IRecaptchaService _recaptchaService;

        public RecaptchaRequestHandler() { }

        public RecaptchaRequestHandler(IRecaptchaService recaptchaService)
        {
            _recaptchaService = recaptchaService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _recaptchaService = scope.Resolve<IRecaptchaService>();
        }

        public async Task<object> VerifyRecaptchaTokenAsync()
        {
            return await _recaptchaService.VerifyRecaptchaTokenAsync(RecaptchaToken);
        }
    }
}
