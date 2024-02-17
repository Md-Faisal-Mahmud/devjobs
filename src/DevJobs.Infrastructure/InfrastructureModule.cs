using Autofac;
using DevJobs.Application;
using DevJobs.Application.Features.JobParsing;
using DevJobs.Application.Features.Services;
using DevJobs.Application.Utilities;
using DevJobs.Domain.Repositories;
using DevJobs.Infrastructure.Features.JobParsing;
using DevJobs.Infrastructure.Features.Membership;
using DevJobs.Infrastructure.Features.Services;
using DevJobs.Infrastructure.Repositories;
using DevJobs.Infrastructure.Securities;
using DevJobs.Infrastructure.Features.Membership.Services;
using DevJobs.Application.Features.Membership.Services;
using DevSkill.Http.Membership;
using Microsoft.AspNetCore.Authorization;
using DevJobs.Infrastructure.Securities.ClaimRequirement;
using DevJobs.Infrastructure.Features.Membership;
using DevJobs.Infrastructure.Utilities;
using DevSkill.Http.Membership;
using System.Diagnostics.CodeAnalysis;
using DevJobs.Infrastructure.Services;
using DevJobs.Application.Services;

namespace DevJobs.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class InfrastructureModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public InfrastructureModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<TokenService>().As<ITokenService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EmailMessageService>().As<IEmailMessageService>().
                InstancePerLifetimeScope();

            builder.RegisterType<LoginService>().As<ILoginService>().
                InstancePerLifetimeScope();

            builder.RegisterType<RecaptchaService>().As<IRecaptchaService>().
                InstancePerLifetimeScope();

            builder.RegisterType<ForgotPasswordService>().As<IForgotPasswordService>().
                InstancePerLifetimeScope();

            builder.RegisterType<ResetPasswordService>().As<IResetPasswordService>().
                InstancePerLifetimeScope();

            builder.RegisterType<AccountUtilities>().As<IAccountUtilities>().
                InstancePerLifetimeScope();

            builder.RegisterType<UserManagerAdapter<ApplicationUser, Guid>>().As<IUserManagerAdapter<ApplicationUser, Guid>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SignInManagerAdapter<ApplicationUser, Guid>>().As<ISignInManagerAdapter<ApplicationUser, Guid>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BdJobsParser>().As<IBdJobsParserService>().
                InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ExperienceMappingRepository>().As<IExperienceMappingRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<JobPostRepository>().As<IJobPostRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TechnologyRepository>().As<ITechnologyRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TrackMappingRepository>().As<ITrackMappingRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserManagerAdapter<ApplicationUser, Guid>>()
                .As<IUserManagerAdapter<ApplicationUser, Guid>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserManagementService>().As<IUserManagementService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserCreateRequirementHandler>().As<IAuthorizationHandler>()
                .InstancePerDependency();

            builder.RegisterType<UserUpdateRequirementHandler>().As<IAuthorizationHandler>()
                .InstancePerDependency();

            builder.RegisterType<UserDeleteRequirementHandler>().As<IAuthorizationHandler>()
                .InstancePerDependency();

            builder.RegisterType<UserViewRequirementHandler>().As<IAuthorizationHandler>()
                .InstancePerDependency();
                
            builder.RegisterType<WorkerServiceStatusCheckService>()
               .As<IWorkerServiceStatusCheckService>().InstancePerLifetimeScope();

            builder.RegisterType<ServiceStatus>()
               .As<IServiceStatus>().InstancePerLifetimeScope();
               
            builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();
            
            builder.RegisterType<DbLogRepository>()
               .As<IDbLogRepository>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}