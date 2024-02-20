using DevJobs.Api.Request;
using FluentValidation;

namespace DevJobs.Api.Validators
{
    public class RecaptchaRequestValidator : AbstractValidator<RecaptchaRequestHandler>
    {
        public RecaptchaRequestValidator()
        {
            RuleFor(x => x.RecaptchaToken)
                .NotNull()
                .NotEmpty()
                .WithMessage("You need to Provide Recaptcha Token. It can't be Empty");
        }
    }
}
