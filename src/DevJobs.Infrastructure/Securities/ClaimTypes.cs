namespace DevJobs.Infrastructure.Securities
{
    public static class ClaimTypes
    {
        public static (string Name, string Value) UserCreateClaim = ("UserCreateClaim", "true");
        public static (string Name, string Value) UserUpdateClaim = ("UserCreateClaim", "true");
        public static (string Name, string Value) UserDeleteClaim = ("UserCreateClaim", "true");
        public static (string Name, string Value) UserViewClaim = ("UserCreateClaim", "true");
        public static (string Name, string Value) DetailsJobViewClaim = ("ViewJobDetails", "true");
        public static (string Name, string Value) JobListViewClaim = ("ViewJobList", "true");
        public static (string Name, string Value) OrganizationViewClaim = ("OrganizationClaim", "true");
        public static (string Name, string Value) ServiceStatusViewClaim = ("ServiceStatusView", "true");
    }
}
