using System.CommandLine;
using HealthCheckerCLI.Commands;

namespace HealthCheckerCLI.Services
{
    public class CLIService
    {
        private const string ROOT_COMMAND_NAME = "HealthCheckerCLI";

        private RootCommand _rootCommand;
        private readonly CheckUrlCommand _checkUrlCommand;
        private readonly StartFileConfigurationCommand _startFileConfigurationCommand;

        public CLIService()
        {
            _rootCommand = new(ROOT_COMMAND_NAME);

            _checkUrlCommand = new(_rootCommand);
            _startFileConfigurationCommand = new(_rootCommand);
        }

        private void InitializeCommands()
        {
            _checkUrlCommand.InitializeCommand();
            _startFileConfigurationCommand.InitializeCommand();
        }

        public async Task StartUp(string[] args)
        {
            InitializeCommands();

            await _rootCommand.InvokeAsync(args).ConfigureAwait(false);
        }
    }
}
