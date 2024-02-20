using DevJobs.Api.Request;
using FluentValidation;

namespace DevJobs.Api.Validators
{
    public class JobPostCountDataRequestValidator:AbstractValidator<JobPostCountDataRequestHandler>
    {
        public JobPostCountDataRequestValidator() 
        {
            RuleFor(x => x.Days)
                .LessThanOrEqualTo(100)
                .WithMessage("Request data can never be more than 100 days");
        }
    }
}
