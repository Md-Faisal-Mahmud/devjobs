using DevSkill.Extensions.Queryable;
using FluentValidation;

namespace DevJobs.Api.Validators
{
    public class UserSearchRequestValidator:AbstractValidator<SearchRequest>
    {
        public UserSearchRequestValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1).WithMessage("Page Index must be greater than or equal to 1");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("Page Size must be greater than or equal to 1");
        }
    }
}
