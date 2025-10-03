using HealthCheckerCLI.Helpers;
using HealthCheckerCLI.Services;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net;

namespace HealthCheckerCLI.Quartz.Workers
{
    public class ServiceChecker
    {
        private static Dictionary<string, int> _attemptCounts = new();
        private readonly TelegramService _telegramService;
        private readonly HttpClient _httpClient = new();
        private readonly ILogger<ServiceChecker> _logger;

        public ServiceChecker(TelegramService telegramService, ILogger<ServiceChecker> logger)
        {
            _telegramService = telegramService;
            _logger = logger;
        }

        async public void PingService(HealthCheckEntry serviceEntry, string serviceName)
        {
            var responseMessage = await _httpClient.GetAsync(new Uri(serviceEntry.Link!)).ConfigureAwait(false);
            HttpStatusCode responsedStatusCode = responseMessage.StatusCode;

            if(serviceEntry.HttpErrorCodes.Contains((int)responsedStatusCode))
            {
                if (!_attemptCounts.ContainsKey(serviceName))
                    _attemptCounts[serviceName] = 1;

                _logger.LogError($"The service `{serviceName}` is unavailable and responded with a status code of {(int)responsedStatusCode}.\n" +
                    $"Attempt to repeat the request... Attempt = {_attemptCounts[serviceName]}");

                if (_attemptCounts[serviceName] == serviceEntry.Attempts)
                {
                    _logger.LogWarning("Отправка сообщения об ошибки в Telegram Bot");
                    _telegramService.SendServiceNotification(serviceName, serviceEntry);
                }
                _attemptCounts[serviceName]++;
            } 
            else
            {
                _attemptCounts[serviceName] = 1;
                _logger.LogInformation($"The service `{serviceName}` responded with a status code of {(int)responsedStatusCode}"); 
            } 
        }
    }
}
