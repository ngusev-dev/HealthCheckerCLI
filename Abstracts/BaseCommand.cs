using System.CommandLine;

namespace HealthCheckerCLI.Abstracts
{
    abstract public class BaseCommand : IBaseCommand
    {
        public abstract void InitializeCommand(RootCommand rootCommand);
    }
}
