using System.CommandLine;

namespace HealthCheckerCLI.Abstracts
{
    abstract public class BaseCommand : IBaseCommand
    {
        private protected RootCommand _rootCommand;

        public BaseCommand(RootCommand rootCommand)
        {
            _rootCommand = rootCommand;
        }

        public abstract void InitializeCommand();
    }
}
