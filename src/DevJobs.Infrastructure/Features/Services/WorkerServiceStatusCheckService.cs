using DevJobs.Application.Features.Services;
using DevJobs.Infrastructure.Utilities;

namespace DevJobs.Infrastructure.Features.Services
{
    public class WorkerServiceStatusCheckService : IWorkerServiceStatusCheckService
    {
        private readonly IServiceStatus _status;
        public WorkerServiceStatusCheckService(IServiceStatus status)
        {
            _status = status;
        }
        public bool CheckServiceStatus()
        {
            return _status.status();
        }
    }
}
