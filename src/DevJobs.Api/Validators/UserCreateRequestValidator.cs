using DevJobs.Application.Features.Membership.DTOs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace DevJobs.Api.Validators
{
    public class UserCreateRequestValidator: AbstractValidator<UserCreateDTO>
    {
        public UserCreateRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .NotNull().WithMessage("First Name is required");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Last Name is required")
                .NotEqual(x => x.FirstName)
                .WithMessage("Last name and first name can not be same");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Username Required")
                .Length(5, 50)
                .WithMessage("Username length between 6-50");

            RuleFor(x => x.PhoneNumber)
                .Must(BeAValidPhoneNumber)
                .WithMessage("Invalid Phone number");
                
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Valid Email is required");

            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("Password is required")
                .Length(6, 32)
                .WithMessage("Password length should be in 6-32");
        }

        private bool BeAValidPhoneNumber(string phoneNumber)
        {
            if (phoneNumber is null) return true;
            string pattern = @"^(\+\d{10,15}|\d{10,15})$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
