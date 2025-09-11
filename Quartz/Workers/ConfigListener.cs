namespace HealthCheckerCLI.Quartz.Workers
{
    static class ConfigListener
    {
        static HttpClient httpClient = new();

        async public static void DisplayDate(string link, int interval, string serviceName)
        {
            var responseMessage = await httpClient.GetAsync(new Uri(link)).ConfigureAwait(false);

            Console.WriteLine($"[{serviceName}] StatusCode = {responseMessage.StatusCode} [{(int)responseMessage.StatusCode}], Interval = {interval}");
        }
    }
}
