using DevJobs.Infrastructure.Features.Membership;
using System.Diagnostics.CodeAnalysis;

namespace DevJobs.Infrastructure.Seeds
{
    [ExcludeFromCodeCoverage]
    internal static class ApplicationUserSeed
    {
        public static IList<ApplicationUser> Users
        {
            get
            {
                return new List<ApplicationUser>
                {
                    new () {
                        Id = new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                        FirstName = "Mr",
                        LastName = "Admin",
                        Email = "admin@devjobs.com",
                        NormalizedEmail = "ADMIN@DEVJOBS.COM",
                        UserName = "admin@devjobs.com",
                        NormalizedUserName = "ADMIN@DEVJOBS.COM",
                        EmailConfirmed = true,
                        PasswordHash = "AQAAAAIAAYagAAAAECGFKYvSkdPYW6Whbul5Q6tTOpj+qYDFOw2pxXrFTEtFyvu3UbWk1EGoCI8LMi8Rnw==",
                        SecurityStamp = "FCRKKDDXJGESVHVL4FJCQ5HY6LZP4OR5",
                        ConcurrencyStamp = "067ee691-1c33-4e92-9dcb-fddd60698f47",
                        PhoneNumber = null,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnd = null,
                        LockoutEnabled = true,
                        AccessFailedCount = 0
                    },
                    new () {
                        Id = new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff"),
                        FirstName = "Mr",
                        LastName = "Developer",
                        Email = "developer@devjobs.com",
                        NormalizedEmail = "DEVELOPER@DEVJOBS.COM",
                        UserName = "developer@devjobs.com",
                        NormalizedUserName = "DEVELOPER@DEVJOBS.COM",
                        EmailConfirmed = true,
                        PasswordHash = "AQAAAAIAAYagAAAAECGFKYvSkdPYW6Whbul5Q6tTOpj+qYDFOw2pxXrFTEtFyvu3UbWk1EGoCI8LMi8Rnw==",
                        SecurityStamp = "FCRKKDDXJGESVHVL4FJCQ5HY6LZP4OR5",
                        ConcurrencyStamp = "067ee691-1c33-4e92-9dcb-fddd60698f48",
                        PhoneNumber = null,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnd = null,
                        LockoutEnabled = true,
                        AccessFailedCount = 0
                    }
                };
            }
        }
    }
}