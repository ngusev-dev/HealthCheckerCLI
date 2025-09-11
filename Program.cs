using HealthCheckerCLI.Services;

namespace HealthCheckerCLI;

class Programm
{
    async static Task Main(string[] args)
    {
        CLIService cli = new();
        await cli.StartUp(args);
    }
}
