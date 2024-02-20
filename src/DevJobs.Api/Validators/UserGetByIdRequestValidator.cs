using DevJobs.Api.Request.UserManagement;
using FluentValidation;

namespace DevJobs.Api.Validators
{
    public class UserGetByIdRequestValidator: AbstractValidator<UserGetByIdRequestHandler>
    {
        public UserGetByIdRequestValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .NotEqual(Guid.Empty).WithMessage("Id cannot be empty");
        }
    }
}
