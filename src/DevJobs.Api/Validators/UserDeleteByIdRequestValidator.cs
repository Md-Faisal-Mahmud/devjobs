using DevJobs.Api.Request.UserManagement;
using FluentValidation;

namespace DevJobs.Api.Validators
{
    public class UserDeleteByIdRequestValidator:AbstractValidator<UserDeleteByIdRequestHandler>
    {
        public UserDeleteByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .NotEqual(Guid.Empty).WithMessage("Id cannot be empty");
        }
    }
}
