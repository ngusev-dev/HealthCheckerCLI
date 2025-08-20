using HealthCheckerCLI.Quartz.Workers;
using Quartz;

namespace HealthCheckerCLI.Quartz
{
    public class Job : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;

            int interval = dataMap.GetInt("interval");
            string link = dataMap.GetString("link") ?? String.Empty;

            return Task.Run(() => ConfigListener.DisplayDate(link, interval));   
        }
    }
}
