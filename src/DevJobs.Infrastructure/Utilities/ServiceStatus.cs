using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.ServiceProcess;

namespace DevJobs.Infrastructure.Utilities
{
    public class ServiceStatus : IServiceStatus
    {
        private readonly IConfiguration _configuration;
        private readonly string _serviceName;

        public ServiceStatus(IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceName = _configuration["WorkerService:ServiceName"];

        }

        public bool status()
        {
            try
            {
                ServiceController sc = new ServiceController(_serviceName);

                ServiceControllerStatus status = sc.Status;

                if (status == ServiceControllerStatus.Running)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}