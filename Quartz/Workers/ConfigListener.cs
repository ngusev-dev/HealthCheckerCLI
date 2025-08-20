namespace HealthCheckerCLI.Quartz.Workers
{
    static class ConfigListener
    {
        public static void DisplayDate(string link, int interval)
        {
            Console.WriteLine($"[{DateTime.Now}] Link = {link}, Interval = {interval}s");
        }
    }
}
