namespace HealthCheckerCLI.Helpers
{
    public class HealthCheckConfiguration
    {
        public Dictionary<string, HealthCheckEntry>? Services { get; set; }
        public NotificationEntry Notifications { get; set; } = new();
        public LoggerEntry Logger { get; set; } = new();
    }

    public class HealthCheckEntry
    {
        public string? Link { get; set; }
        public int Interval { get; set; } = 10;
        public int Attempts { get; set; } = 1;
        public int[] HttpErrorCodes { get; set; } = new int[] { 404, 500, 501, 502, 503, 504, 505, 506, 507, 508, 510, 511 };
    }

    public class NotificationEntry
    {
        public string TgBotKey { set; get; } = String.Empty;
        public string MessageTemplate { set; get; } = "Дефолтный шаблон";
    }

    public class LoggerEntry
    {
        public bool logInFile { get; set; } = false;
        public string? filePath { get; set; }
    }
}
