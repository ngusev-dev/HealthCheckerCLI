using HealthCheckerCLI.Helpers;
using HealthCheckerCLI.Services;
using System.Net;

namespace HealthCheckerCLI.Quartz.Workers
{
    public class ServiceChecker
    {
        private HttpClient httpClient = new();
        private static Dictionary<string, int> attemptCounts = new();

        private readonly TelegramService _telegramService;

        public ServiceChecker(TelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        async public void PingService(HealthCheckEntry serviceEntry, string serviceName)
        {
            var responseMessage = await httpClient.GetAsync(new Uri(serviceEntry.Link!)).ConfigureAwait(false);
            HttpStatusCode responsedStatusCode = responseMessage.StatusCode;

            if(serviceEntry.HttpErrorCodes.Contains((int)responsedStatusCode))
            {
                if (!attemptCounts.ContainsKey(serviceName))
                    attemptCounts[serviceName] = 1;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"[{DateTime.Now.ToString()}][ERROR] The service `{serviceName}` is unavailable and responded with a status code of {(int)responsedStatusCode}.\n" +
                    $"Attempt to repeat the request... Attempt = {attemptCounts[serviceName]}"
                    );

                if (attemptCounts[serviceName] == serviceEntry.Attempts)
                {
                    _telegramService.SendNotification();
                }
                attemptCounts[serviceName]++;
            } 
            else
            {
                attemptCounts[serviceName] = 1;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now.ToString()}][INFO] The service `{serviceName}` responded with a status code of {(int)responsedStatusCode}");
            } 
        }
    }
}
