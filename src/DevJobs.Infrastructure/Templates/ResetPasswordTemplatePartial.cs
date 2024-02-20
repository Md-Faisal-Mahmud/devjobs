namespace DevJobs.Infrastructure.Templates
{
    public partial class ResetPasswordTemplate(string name, string link)
    {
        private string Name { get; } = name;
        private string Link { get; } = link;
    }
}