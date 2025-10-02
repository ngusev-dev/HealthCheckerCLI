using Quartz;
using System.Text.Json;
using HealthCheckerCLI.Helpers;
using HealthCheckerCLI.Quartz.Workers;

namespace HealthCheckerCLI.Quartz
{
    public class CheckingServiceJob : IJob
    {
        private ServiceChecker _serviceChecker;

        public CheckingServiceJob(ServiceChecker serviceChecker)
        {
            _serviceChecker = serviceChecker;
        }

        public Task Execute(IJobExecutionContext context)
        {
            HealthCheckEntry? service = JsonSerializer.Deserialize<HealthCheckEntry>(context.MergedJobDataMap.GetString("service")!);
            string serviceName = context.MergedJobDataMap.GetString("serviceName") ?? String.Empty;

            if (service == null) return Task.CompletedTask;

            return Task.Run(() => _serviceChecker.PingService(service, serviceName));   
        }
    }
}
