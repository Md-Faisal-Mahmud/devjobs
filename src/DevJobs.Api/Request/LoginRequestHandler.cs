using Autofac;
using DevJobs.Application.Features.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevJobs.Api.Request
{
    public class LoginRequestHandler
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        private ILoginService _loginService;

        public LoginRequestHandler() { }

        public LoginRequestHandler(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _loginService = scope.Resolve<ILoginService>();
        }

        public async Task<object> LoginUserAsync()
        {
            return await _loginService.LoginUserAsync(Email, Password, RememberMe);
        }
    }
}
