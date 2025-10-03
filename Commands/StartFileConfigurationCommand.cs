using HealthCheckerCLI.Abstracts;
using HealthCheckerCLI.Services;
using Quartz;
using System.CommandLine;
using System.CommandLine.Invocation;
using HealthCheckerCLI.Quartz;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace HealthCheckerCLI.Commands
{
    public class StartFileConfigurationCommand : BaseCommand
    {
        private IScheduler _scheduler;
        private readonly ConfigurationService _configurationService;
        private readonly ILogger<StartFileConfigurationCommand> _logger;

        public StartFileConfigurationCommand(IScheduler scheduler, ConfigurationService configurationService, ILogger<StartFileConfigurationCommand> logger)
        {
            _scheduler = scheduler;
            _configurationService = configurationService;
            _logger = logger;
        }

        public override void InitializeCommand(RootCommand rootCommand)
        {
            Command startByFileCommand = new("start", "Start checking services using YAML-file (by default, health-cli.yaml)");

            startByFileCommand.Handler = CommandHandler.Create(async () =>
            {
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

                    _logger.LogInformation($"Scheduler started for service [{service.Key}]");
                }
                
                Console.WriteLine("HealthChecker has been successfully launched!");

                Console.ReadKey();
                await _scheduler.Shutdown();
                _logger.LogInformation($"Scheduler stopped.");
            });

            rootCommand.AddCommand(startByFileCommand);
        }
    }
}
