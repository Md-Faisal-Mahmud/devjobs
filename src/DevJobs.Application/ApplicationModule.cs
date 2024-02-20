using Autofac;
using DevJobs.Application.Features.DbLogViewing;
using DevJobs.Application.Features.DbLogViewing.Services;
using DevJobs.Application.Features.JobPostAnalyzing.Services;
using DevJobs.Application.Features.JobPostVisualizing.Services;
using DevJobs.Application.Features.OrganizationList;

namespace DevJobs.Application
{
    public class ApplicationModule : Module
    {
        public ApplicationModule() { }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JobPostQueryService>().As<IJobPostQueryService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<JobPostCountService>().As<IJobPostCountService>().InstancePerLifetimeScope();

            builder.RegisterType<OrganizationListQueryService>().As<IOrganizationListQueryService>().InstancePerLifetimeScope();

            builder.RegisterType<DbLogQueryService>().As<IDbLogQueryService>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
