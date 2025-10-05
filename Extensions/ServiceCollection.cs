using HealthCheckerCLI.Commands;
using HealthCheckerCLI.Quartz;
using HealthCheckerCLI.Quartz.Workers;
using HealthCheckerCLI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Serilog;

namespace HealthCheckerCLI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Configure(this IServiceCollection services)
        {
            services.ConfigureLogger();
            services.ConfigureHttpClient();
            services.ConfigureServices();
            services.ConfigureCommands();
            services.ConfigureQuartz();

            services.AddScoped<ServiceChecker>();

            return services;
        }

        public static IServiceCollection ConfigureHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient("tg-api", c =>
            {
                c.BaseAddress = new("https://api.telegram.org/");
            });

            return services;
        }

        public static IServiceCollection ConfigureLogger(this IServiceCollection services)
        {
            services.AddSingleton<Serilog.ILogger>((provider) =>
            {
                var configService = provider.GetRequiredService<ConfigurationService>();
                var loggerConfig = new LoggerConfiguration();

                if (configService.configurationFile != null)
                {
                    if(configService.configurationFile.Logger.logInFile)
                        loggerConfig = loggerConfig.WriteTo.File(configService.configurationFile.Logger.filePath, rollingInterval: RollingInterval.Day);

                    if(configService.configurationFile.Logger.consoleMode)
                        loggerConfig = loggerConfig.WriteTo.Console();
                }

                var logger = loggerConfig.CreateLogger();
                Log.Logger = logger;

                return logger;
            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<CLIService>();
            services.AddSingleton<TelegramService>();
            services.AddSingleton<ConfigurationService>();

            return services;
        }

        public static IServiceCollection ConfigureCommands(this IServiceCollection services)
        {
            services.AddTransient<CheckUrlCommand>();
            services.AddTransient<StartFileConfigurationCommand>();

            return services;
        }

        public static IServiceCollection ConfigureQuartz(this IServiceCollection services)
        {
            services.AddQuartz();
            services.AddSingleton(provider => provider.GetRequiredService<ISchedulerFactory>().GetScheduler().GetAwaiter().GetResult());
            services.AddTransient<CheckingServiceJob>();

            return services;
        }
    }
}
