using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevJobs.DataCollectorService;
using DevJobs.Infrastructure;
using Serilog;
using Serilog.Events;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

var assemblyName = typeof(Worker).Assembly.FullName;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(configuration)
    .Enrich.WithProperty("MachineName", Environment.MachineName)
    .CreateLogger();

try
{
    IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .UseSerilog()
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule(new DataCollectorServiceModule());
            builder.RegisterModule(new InfrastructureModule(connectionString, assemblyName));
        })
        .ConfigureServices(services =>
        {
            services.AddHostedService<Worker>();
        })
        .Build();

    Log.Information("Application starting up!");

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Error(ex, "An error occurred: {ErrorMessage}", ex.Message);
}
finally
{
    Log.CloseAndFlush();
}