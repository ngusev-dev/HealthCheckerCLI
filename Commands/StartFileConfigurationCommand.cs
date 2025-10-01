using HealthCheckerCLI.Abstracts;
using HealthCheckerCLI.Services;
using Quartz;
using System.CommandLine;
using System.CommandLine.Invocation;
using HealthCheckerCLI.Quartz;
using System.Text.Json;

namespace HealthCheckerCLI.Commands
{
    public class StartFileConfigurationCommand : BaseCommand
    {
        private IScheduler _scheduler;
        private readonly ConfigurationService _configurationService;

        public StartFileConfigurationCommand(IScheduler scheduler, ConfigurationService configurationService)
        {
            _scheduler = scheduler;
            _configurationService = configurationService;
        }

        public override void InitializeCommand(RootCommand rootCommand)
        {
            Command startByFileCommand = new("start", "Start checking services using YAML-file (by default, health-cli.yaml)");

            startByFileCommand.Handler = CommandHandler.Create(async () =>
            {
                Console.WriteLine(_configurationService.configurationFile);
                if (_configurationService.configurationFile?.Services is null) return;

                foreach (var service in _configurationService.configurationFile.Services)
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

                    await _scheduler.ScheduleJob(job, trigger);

                    await _scheduler.Start();

                    Console.WriteLine($"Scheduler started for service [{service.Key}]. Press any key to stop...");
                }


                Console.ReadKey();
                await _scheduler.Shutdown();
                Console.WriteLine("Scheduler stopped.");
            });

            rootCommand.AddCommand(startByFileCommand);
        }
    }
}
