using HealthCheckerCLI.Abstracts;
using HealthCheckerCLI.Services;
using Quartz.Impl;
using Quartz;
using System.CommandLine;
using System.CommandLine.Invocation;
using HealthCheckerCLI.Quartz;

namespace HealthCheckerCLI.Commands
{
    public class StartFileConfigurationCommand : BaseCommand
    {
        public StartFileConfigurationCommand(RootCommand rootCommand) : base(rootCommand) { }

        public override void InitializeCommand()
        {
            Command startByFileCommand = new("start", "Start checking services using YAML-file (by default, config.yaml)");

            startByFileCommand.Handler = CommandHandler.Create(async () =>
            {
                var yamlService = new ConfigurationService();

                var config = await yamlService.GetDeserializeConfig();

                if (config == null || config.Services == null) return;

                IScheduler scheduler = await new StdSchedulerFactory().GetScheduler();

                foreach (var item in config.Services)
                {
                    IJobDetail job = JobBuilder.Create<Job>()
                    .WithIdentity($"{item.Key}-Job", $"{item.Key}-Group")
                    .UsingJobData("interval", item.Value.Interval)
                    .UsingJobData("link", item.Value.Link)
                    .UsingJobData("serviceName", item.Key)
                    .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"{item.Key}-Trigger", $"{item.Key}-Group")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(item.Value.Interval)
                        .RepeatForever())
                    .Build();

                    await scheduler.ScheduleJob(job, trigger);

                    await scheduler.Start();

                    Console.WriteLine($"Scheduler started for service [{item.Key}]. Press any key to stop...");
                }


                Console.ReadKey();
                await scheduler.Shutdown();
                Console.WriteLine("Scheduler stopped.");
            });

            _rootCommand.AddCommand(startByFileCommand);
        }
    }
}
