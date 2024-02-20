using DevJobs.Infrastructure.Features.Membership;
using System.Diagnostics.CodeAnalysis;

namespace DevJobs.Infrastructure.Seeds
{
    [ExcludeFromCodeCoverage]
    internal static class ApplicationUserRolesSeed
    {
        public static IList<ApplicationUserRole> UserRoles
        {
            get
            {
                return new List<ApplicationUserRole>
                {
                    new () {
                        UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        RoleId = new Guid("77c3aef9-9444-4c71-2db4-08dc050745fe")
                    },
                    new () {
                        UserId = new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff"),
                        RoleId = new Guid("78c3aef9-9444-4c71-2db4-08dc050745ff")
                    }
                };
            }
        }
    }
}