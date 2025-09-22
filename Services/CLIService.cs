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

        public CLIService(
            CheckUrlCommand checkUrlCommand,
            StartFileConfigurationCommand startFileConfigurationCommand
            )
        {
            _rootCommand = new(ROOT_COMMAND_NAME);

            _checkUrlCommand = checkUrlCommand;
            _startFileConfigurationCommand = startFileConfigurationCommand;
        }

        private void InitializeCommands()
        {
            _checkUrlCommand.InitializeCommand(_rootCommand);
            _startFileConfigurationCommand.InitializeCommand(_rootCommand);
        }

        public async Task StartUp(string[] args)
        {
            InitializeCommands();
            await _rootCommand.InvokeAsync(args).ConfigureAwait(false);
        }
    }
}
