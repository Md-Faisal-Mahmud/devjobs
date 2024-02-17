using DevJobs.Application.Features.Membership.DTOs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace DevJobs.Api.Validators
{
    public class UserUpdateRequestValidator: AbstractValidator<UserDetailsDTO>
    {
        public UserUpdateRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .Length(1, 50).WithMessage("First Name length should be between 1 and 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .Length(1, 50).WithMessage("Last Name length should be between 1 and 50 characters");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .Length(5, 50).WithMessage("Username length should be between 5 and 50 characters");

            RuleFor(x => x.PhoneNumber)
                .Must(BeAValidPhoneNumber)
                .WithMessage("Invalid Phone number");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid Email Address");

            RuleFor(x => x.AccessFailedCount)
                .GreaterThanOrEqualTo(0).WithMessage("Access Failed Count should be non-negative");
            
            RuleFor(x => x.LockoutEnd)
               .Must(BeAValidLockoutEndDate).WithMessage("Invalid Lockout End Date");

            RuleFor(x => x.AccessFailedCount)
                .GreaterThanOrEqualTo(0).WithMessage("Access Failed Count should be non-negative");
        }

        private bool BeAValidLockoutEndDate(DateTimeOffset? lockoutEnd)
        {
            return lockoutEnd == null || lockoutEnd > DateTimeOffset.UtcNow;
        }
        private bool BeAValidPhoneNumber(string phoneNumber)
        {
            if (phoneNumber is null) return true;
            string pattern = @"^(\+\d{10,15}|\d{10,15})$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

    }
}
