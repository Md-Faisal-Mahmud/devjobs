using Autofac;
using DevJobs.Application.Features.Services;

namespace DevJobs.Api.Request
{
    public class WorkerServiceStatusRequestHandler
    {
        private IWorkerServiceStatusCheckService _workerServiceStatusCheckService;
        
        public WorkerServiceStatusRequestHandler()
        {

        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _workerServiceStatusCheckService = scope.Resolve<IWorkerServiceStatusCheckService>();
        }

        public bool CheckServiceStatus()
        {
            return _workerServiceStatusCheckService.CheckServiceStatus();
        }
    }
}
