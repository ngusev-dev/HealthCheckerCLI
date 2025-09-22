using HealthCheckerCLI.Extensions;
using HealthCheckerCLI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HealthCheckerCLI;

class Programm
{
    async static Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services.ConfigureServices();

        ServiceProvider provider = services.BuildServiceProvider();
        CLIService cli = provider.GetRequiredService<CLIService>();

        await cli.StartUp(args).ConfigureAwait(false);
    }
}
