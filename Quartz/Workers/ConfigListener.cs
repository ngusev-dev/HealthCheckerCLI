using HealthCheckerCLI.Helpers;
using HealthCheckerCLI.Services;
using System.Net;

namespace HealthCheckerCLI.Quartz.Workers
{
    public class ServiceChecker
    {
        private static Dictionary<string, int> _attemptCounts = new();
        private readonly TelegramService _telegramService;
        private readonly HttpClient _httpClient = new();

        public ServiceChecker(TelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        async public void PingService(HealthCheckEntry serviceEntry, string serviceName)
        {
            var responseMessage = await _httpClient.GetAsync(new Uri(serviceEntry.Link!)).ConfigureAwait(false);
            HttpStatusCode responsedStatusCode = responseMessage.StatusCode;

            if(serviceEntry.HttpErrorCodes.Contains((int)responsedStatusCode))
            {
                if (!_attemptCounts.ContainsKey(serviceName))
                    _attemptCounts[serviceName] = 1;

                ScreenLogHelper.Error($"[{DateTime.Now.ToString()}][ERROR] The service `{serviceName}` is unavailable and responded with a status code of {(int)responsedStatusCode}.\n" +
                    $"Attempt to repeat the request... Attempt = {_attemptCounts[serviceName]}");

                if (_attemptCounts[serviceName] == serviceEntry.Attempts)
                {
                    ScreenLogHelper.Information("Отправка сообщения об ошибки в Telegram Bot");
                    _telegramService.SendServiceNotification(serviceName, serviceEntry);
                }
                _attemptCounts[serviceName]++;
            } 
            else
            {
                _attemptCounts[serviceName] = 1;
                ScreenLogHelper.Success($"[{DateTime.Now.ToString()}][INFO] The service `{serviceName}` responded with a status code of {(int)responsedStatusCode}");
            } 
        }
    }
}
