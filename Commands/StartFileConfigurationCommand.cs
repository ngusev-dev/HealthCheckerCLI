using HealthCheckerCLI.Abstracts;
using HealthCheckerCLI.Services;
using Quartz.Impl;
using Quartz;
using System.CommandLine;
using System.CommandLine.Invocation;
using HealthCheckerCLI.Quartz;
using System.Text.Json;

namespace HealthCheckerCLI.Commands
{
    public class StartFileConfigurationCommand : BaseCommand
    {
        public StartFileConfigurationCommand(RootCommand rootCommand) : base(rootCommand) { }

        public override void InitializeCommand()
        {
            Command startByFileCommand = new("start", "Start checking services using YAML-file (by default, health-cli.yaml)");

            startByFileCommand.Handler = CommandHandler.Create(async () =>
            {
                var yamlService = new ConfigurationService();
                var config = await yamlService.GetDeserializeConfig();
                if (config == null || config.Services == null) return;

                IScheduler scheduler = await new StdSchedulerFactory().GetScheduler();

                foreach (var service in config.Services)
                {
                    IJobDetail job = JobBuilder.Create<CheckingServiceJob>()
                    .WithIdentity($"{service.Key}-Job", $"{service.Key}-Group")
                    .UsingJobData("service", JsonSerializer.Serialize(service.Value))
                    .UsingJobData("serviceName", service.Key)
                    .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"{service.Key}-Trigger", $"{service.Key}-Group")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(service.Value.Interval)
                        .RepeatForever())
                    .Build();

                    await scheduler.ScheduleJob(job, trigger);

                    await scheduler.Start();

                    Console.WriteLine($"Scheduler started for service [{service.Key}]. Press any key to stop...");
                }


                Console.ReadKey();
                await scheduler.Shutdown();
                Console.WriteLine("Scheduler stopped.");
            });

            _rootCommand.AddCommand(startByFileCommand);
        }
    }
}
