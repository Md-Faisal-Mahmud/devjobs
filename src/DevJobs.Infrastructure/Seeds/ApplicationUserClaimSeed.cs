using DevJobs.Infrastructure.Features.Membership;
using System.Diagnostics.CodeAnalysis;

namespace DevJobs.Infrastructure.Seeds
{
    [ExcludeFromCodeCoverage]
    internal static class ApplicationUserClaimSeed
    {
        public static IList<ApplicationUserClaim> Claims
        {
            get
            {
                return new List<ApplicationUserClaim>()
                {
                    new ApplicationUserClaim()
                    {
                        Id = 2,
                        UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        ClaimType = "ViewJobList",
                        ClaimValue = "true"
                    },

                    new ApplicationUserClaim()
                    {
                        Id = 3, UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        ClaimType = "ViewJobDetails",
                        ClaimValue = "true"
                    },

                    new ApplicationUserClaim()
                    {
                        Id = 1,
                        UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        ClaimType = "OrganizationClaim",
                        ClaimValue = "true"
                    },

                    new ApplicationUserClaim()
                    {
                        Id = 5,
                        UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        ClaimType = "UserCreateClaim",
                        ClaimValue = "true"
                    },

                    new ApplicationUserClaim()
                    {
                        Id = 6,
                        UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        ClaimType = "UserUpdateClaim",
                        ClaimValue = "true"
                    },

                    new ApplicationUserClaim()
                    {
                        Id = 7,
                        UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        ClaimType = "UserDeleteClaim",
                        ClaimValue = "true"
                    },

                    new ApplicationUserClaim()
                    {
                        Id = 8,
                        UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        ClaimType = "UserViewClaim",
                        ClaimValue = "true"
                    },

                    new ApplicationUserClaim()
                    {
                        Id = 9,
                        UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff"),
                        ClaimType = "UserViewClaim",
                        ClaimValue = "true"
                    },
                    new ApplicationUserClaim()
                    {
                        Id = 4, UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        ClaimType = "ServiceStatusView",
                        ClaimValue = "true"
                    },
                    new ApplicationUserClaim()
                    {
                        Id = 10, UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        ClaimType = "LogListViewRequirementPolicy",
                        ClaimValue = "true"
                    },
                    new ApplicationUserClaim()
                    {
                        Id = 11, UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        ClaimType = "LogListViewRequirementPolicy",
                        ClaimValue = "true"
                    },
                    new ApplicationUserClaim()
                    {
                        Id = 12, UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        ClaimType = "DeleteLogByIdRequirementPolicy",
                        ClaimValue = "true"
                    }
                };
            }
        }
    }
}