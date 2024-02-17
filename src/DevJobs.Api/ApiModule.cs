using Autofac;
using DevJobs.Api.Request;
using DevJobs.Api.Request.UserManagement;

namespace DevJobs.Api
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginRequestHandler>().AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<RecaptchaRequestHandler>().AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<JobPostCountDataRequestHandler>().AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<ForgotPasswordRequestHandler>().AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<ResetPasswordRequestHandler>().AsSelf()
                .InstancePerLifetimeScope();

            // User Management
            builder.RegisterType<UserCreateRequestHandler>().AsSelf()
                .InstancePerDependency();

            builder.RegisterType<UserUpdateRequestHandler>().AsSelf()
                .InstancePerDependency();

            builder.RegisterType<UserGetRequestHandler>().AsSelf()
                .InstancePerDependency();

            builder.RegisterType<UserGetByIdRequestHandler>().AsSelf()
                .InstancePerDependency();

            builder.RegisterType<UserDeleteByIdRequestHandler>().AsSelf()
                .InstancePerDependency();
                
                //Job Post
            builder.RegisterType<JobPostListRequestHandler>().AsSelf();
            
            builder.RegisterType<OrganizationListRequestHandler>().AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<WorkerServiceStatusRequestHandler>().AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<JobPostGetRequestHandler>().AsSelf();

            builder.RegisterType<DbLogRequestHandler>().AsSelf();

            builder.RegisterType<DbLogListRequestHandler>().AsSelf();

            base.Load(builder);
        }
    }
}
