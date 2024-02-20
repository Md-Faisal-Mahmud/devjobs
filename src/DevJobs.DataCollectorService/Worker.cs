using DevJobs.Application.Features.JobParsing;

namespace DevJobs.DataCollectorService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IBdJobsParserService _bdJobsParserService;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IBdJobsParserService parser,
            IConfiguration configuration, IBdJobsParserService bdJobsParserService)
        {
            _logger = logger;
            _bdJobsParserService = bdJobsParserService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        await _bdJobsParserService.ParseAllJobsAsync();
                        _logger.LogInformation("Alhamdulillah, All job posts parsing is complete at: {time}", DateTimeOffset.Now);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in the worker service.");
                }
                finally
                {
                    var jobUpdateDurationMinutes = _configuration.GetValue<int>("DataCollectorService:JobUpdateDurationMinutes");
                    await Task.Delay(TimeSpan.FromMinutes(jobUpdateDurationMinutes), stoppingToken);
                }
            }
        }
    }
}