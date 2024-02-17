using DevJobs.Api.Request;
using FluentValidation;

namespace DevJobs.Api.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestHandler>
    {
        public LoginRequestValidator()
        {
            RuleFor(x=> x.Email)
                .NotNull()
                .NotEmpty().WithMessage("You have to provide an Email")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty().WithMessage("Password Shouldn't be Empty");
            RuleFor(x => x.RememberMe)
                .NotNull().WithMessage("RememberMe must be specified");
        }
    }
}
