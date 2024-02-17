using DevJobs.Application.Features.Services;
using DevJobs.Application.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace DevJobs.Infrastructure.Features.Services
{
    public class RecaptchaService(IConfiguration configuration, IAccountUtilities accountUtilities) : IRecaptchaService
    {
        private string SecretKey => configuration["ReCaptcha:SecretKey"]!;

        public async Task<object> VerifyRecaptchaTokenAsync(string recaptchaToken)
        {
            var isSuccess = await accountUtilities.VerifyRecaptchaToken(SecretKey, recaptchaToken);
            if (isSuccess)
                return accountUtilities.GetTrueReturn();
            else
                return accountUtilities.GetFalseReturn();
        }
    }
}
