namespace HealthCheckerCLI.Services
{
    public class TelegramService
    {
        private readonly ConfigurationService _configurationService;

        public TelegramService(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public void SendNotification()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(_configurationService.configurationFile?.Notifications.MessageTemplate);
        }
    }
}
