using DevJobs.Api.Request;
using FluentValidation;

namespace DevJobs.Api.Validators
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequestHandler>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty().WithMessage("UserId must be specified");

            RuleFor(x => x.Code)
                .NotNull()
                .NotEmpty().WithMessage("Code must be specified");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty().WithMessage("Password must be specified")
                .MinimumLength(6).WithMessage("Password should have at least 6 characters");

            RuleFor(x => x.ConfirmPassword)
                .NotNull()
                .NotEmpty().WithMessage("ConfirmPassword must be specified")
                .Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");
        }
    }
}