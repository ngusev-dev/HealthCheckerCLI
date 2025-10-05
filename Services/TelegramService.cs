using HealthCheckerCLI.Helpers;
using System.Net;

namespace HealthCheckerCLI.Services
{
    public class TelegramService
    {
        private readonly ConfigurationService _configurationService;
        private readonly HttpClient _tgClient;

        public TelegramService(ConfigurationService configurationService, IHttpClientFactory httpClientFactory)
        {
            _configurationService = configurationService;
            _tgClient = httpClientFactory.CreateClient("tg-api");
        }

        public bool SendServiceNotification(string serviceName, HealthCheckEntry serviceEntry)
        {
            try
            {
                var cfgNotifications = _configurationService.configurationFile?.Notifications;
                
                string message = cfgNotifications!.MessageTemplate
                    .Replace("{{SERVICE_NAME}}", serviceName)
                    .Replace("{{SERVICE_LINK}}", serviceEntry.Url);

                using HttpRequestMessage requset = new(
                    HttpMethod.Get,
                    $"/bot{cfgNotifications?.TgBotKey}/sendMessage?chat_id=746446233&text={message}");

                HttpResponseMessage status = _tgClient.Send(requset);

                return status.StatusCode == HttpStatusCode.OK; 
            } 
            catch 
            {
                throw new Exception("Ошибка при отправке сообщения через Telegram Bot");
            }
        }
    }
}
