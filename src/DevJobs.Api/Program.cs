using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevJobs.Api;
using DevJobs.Api.Settings;
using DevJobs.Application;
using DevJobs.Infrastructure;
using DevJobs.Infrastructure.Extensions;
using DevJobs.Infrastructure.Securities;
using DevJobs.Infrastructure.Securities.ClaimRequirement;
using DevJobs.Infrastructure.Securities.DbLogViewRequirement;
using DevJobs.Infrastructure.Securities.JobDetailsViewRequirement;
using DevJobs.Infrastructure.Securities.JobListViewRequirement;
using DevSkill.Http.Emails;
using DevSkill.Http.Emails.BusinessObjects;
using DevSkill.Http.Emails.Contexts;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;
using DevJobs.Infrastructure.Securities.ClaimRequirement;
using DevSkill.Extensions.FileStorage;
using DevSkill.Extensions.FileStorage.Enums;
using DevSkill.Extensions.FileStorage.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration));

// Add services to the container.
try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new ApiModule());
        containerBuilder.RegisterModule(new ApplicationModule());
        containerBuilder.RegisterModule(new InfrastructureModule(connectionString!,
            migrationAssembly!));
        containerBuilder.RegisterModule(new EmailMessagingModule(connectionString!,
            migrationAssembly!));
    });

    builder.Services.AddFileStorage(builder.Configuration, opt =>
    {
        opt.Mode = FileStorageMode.AmazonS3;
        opt.WebRootPath = builder.Environment.WebRootPath;
        opt.ConfigureAmazonS3("S3Config");
    });
    builder.Services.Configure<FileStorageSetting>(builder.Configuration.GetSection("FileStorageSetting"));

    builder.Services.AddDbContext<EmailMessagingContext>(context =>
    {
        context.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(Program).Assembly.FullName));
    });

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddIdentity();

    builder.Services.AddDbContext<ApplicationDbContext>(context =>
    {
        context.UseSqlServer(connectionString, x => x.MigrationsAssembly(migrationAssembly));
    });

    builder.Services.AddAuthentication()
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions =>
        {
            configureOptions.RequireHttpsMetadata = false;
            configureOptions.SaveToken = true;
            configureOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"]
            };
        });

    builder.Services.AddAuthorization(option =>
    {
        option.AddPolicy("JobViewRequirementPolicy", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new JobViewRequirement());
        });
        option.AddPolicy("ServiceStatusView", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new WorkerServiceStatusViewRequirement());
        });
        option.AddPolicy("JobListViewRequirementPolicy", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new JobListViewRequirement());
        });

        option.AddPolicy("JobDetailsViewRequirementPolicy", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new JobDetailsViewRequirement());
        });

        option.AddPolicy("UserCreatePolicy", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new UserCreateRequirement());
        });

        option.AddPolicy("UserUpdatePolicy", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new UserUpdateRequirement());
        });

        option.AddPolicy("UserDeletePolicy", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new UserDeleteRequirement());
        });
        option.AddPolicy("UserViewPolicy", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new UserViewRequirement());
        });
        option.AddPolicy("LogListViewRequirementPolicy", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new DbLogListViewRequirement());
        });

        option.AddPolicy("LogDetailsViewRequirementPolicy", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new DbLogDetailsViewRequirement());
        });
        option.AddPolicy("DeleteLogByIdRequirementPolicy", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new DbLogDeleteByIdRequirement());
        });        
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowWebApp",
            builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
    });

    builder.Services.AddControllers()
        .AddFluentValidation(c =>
        c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

    builder.AddSwagger();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingleton<IAuthorizationHandler, JobViewRequirementHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, WorkerServiceStatusViewRequirementHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, JobListViewRequirementHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, JobDetailsViewRequirementHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, DbLogListViewRequirementHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, DbLogDetailsViewRequirementHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, DbLogDeleteByIdRequirementHandler>();    

    builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("Smtp"));
    var app = builder.Build();

    Log.Information("Application is starting...");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Devjob Api Server V1");
            c.DocumentTitle = "Devjob Api Server";
            c.DocExpansion(DocExpansion.None);
        });
    }

    app.UseCors();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed.");
}
finally
{
    Log.CloseAndFlush();
}
