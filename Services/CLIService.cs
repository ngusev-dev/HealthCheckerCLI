using System.CommandLine.Invocation;
using System.CommandLine;
using Quartz;
using Quartz.Impl;
using HealthCheckerCLI.Quartz;

namespace HealthCheckerCLI.Services
{
    public class CLIService
    {
        private const string ROOT_COMMAND_NAME = "HealthCheckerCLI";

        private RootCommand _rootCommand;

        public CLIService()
        {
            _rootCommand = new(ROOT_COMMAND_NAME);
        }

        private void InitializeCommands()
        {

            Command checkedLinkCommand = new("checked", "Checked response stutus code of website");

            Argument<string> linkArgument = new("link")
            {
                Description = "The link for which need to get response status code",
                Arity = ArgumentArity.ExactlyOne,
            };

            checkedLinkCommand.AddArgument(linkArgument);

            checkedLinkCommand.Handler = CommandHandler.Create<string>(async (link) =>
            {
                HttpClient client = new();
                
                var responseMessage = await client.GetAsync(new Uri(link)).ConfigureAwait(false);

                Console.WriteLine($"Status code for '{link}' = [{(int)responseMessage.StatusCode}] {responseMessage.StatusCode}");
            });

            _rootCommand.AddCommand(checkedLinkCommand);

            Command startByFileCommand = new("start", "Start checking services using YAML-file (by default, config.yaml)");

            startByFileCommand.Handler = CommandHandler.Create(async () =>
            {
                var yamlService = new YamlService();

                var config = await yamlService.GetDeserializeConfig();

                if (config == null) return;

                IScheduler scheduler = await new StdSchedulerFactory().GetScheduler();

                foreach(var item in config)
                {
                    IJobDetail job = JobBuilder.Create<Job>()
                    .WithIdentity($"{item.Key}-Job", $"{item.Key}-Group")
                    .UsingJobData("interval", item.Value.Interval)
                    .UsingJobData("link", item.Value.Link)
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

        public async Task StartUp(string[] args)
        {
            InitializeCommands();
            await _rootCommand.InvokeAsync(args).ConfigureAwait(false);
        }
    }
}
