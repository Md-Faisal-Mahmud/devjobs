using DevJobs.Api.Request;
using FluentValidation;

namespace DevJobs.Api.Validators
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequestHandler>
    {
        public ForgotPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty().WithMessage("You have to provide an Email")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.Origin)
                .NotEmpty().WithMessage("Origin must be specified");
        }
    }
}