using DevJobs.Infrastructure.Features.Membership;
using System.Diagnostics.CodeAnalysis;

namespace DevJobs.Infrastructure.Seeds
{
    [ExcludeFromCodeCoverage]
    internal static class ApplicationRolesSeed
    {
        public static IList<ApplicationRole> Roles
        {
            get
            {
                return new List<ApplicationRole>
                {
                    new () {
                        Id = new Guid("77c3aef9-9444-4c71-2db4-08dc050745fe"),
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        ConcurrencyStamp = "067ee691-1c33-4e92-9dcb-fddd60698f51"
                    },
                    new () {
                        Id = new Guid("78c3aef9-9444-4c71-2db4-08dc050745ff"),
                        Name = "Developer",
                        NormalizedName = "DEVELOPER",
                        ConcurrencyStamp = "067ee691-1c33-4e92-9dcb-fddd60698f52"
                    }
                };
            }
        }
    }
}