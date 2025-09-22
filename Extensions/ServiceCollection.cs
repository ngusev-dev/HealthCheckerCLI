using HealthCheckerCLI.Commands;
using HealthCheckerCLI.Quartz;
using HealthCheckerCLI.Quartz.Workers;
using HealthCheckerCLI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HealthCheckerCLI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<CLIService>();

            services.AddTransient<CheckUrlCommand>();
            services.AddTransient<StartFileConfigurationCommand>();

            services.AddScoped<ServiceChecker>();

            services.AddLogging(logging =>
            {
                logging.AddConsole();
            });

            services.AddQuartz();
            services.AddSingleton(provider => provider.GetRequiredService<ISchedulerFactory>().GetScheduler().GetAwaiter().GetResult());
            services.AddTransient<CheckingServiceJob>();

            return services;
        }
    }
}
