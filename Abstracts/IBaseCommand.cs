using System.CommandLine;

namespace HealthCheckerCLI.Abstracts
{
    interface IBaseCommand
    {
        /// <summary>
        /// Инациализация команды CLI
        /// </summary>
        public void InitializeCommand(RootCommand rootCommand);
    }
}
