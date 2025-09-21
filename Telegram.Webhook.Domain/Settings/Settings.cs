using A2ASMS.Utility.Logger;

namespace Telegram.Webhook.Domain.Settings;

public class DbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string CommandTimeOut { get; set; } = "30";
}

public class AppSettings
{
    public string LogPath { get; set; } = string.Empty;
    public int LogFlushInterval { get; set; } = 0;
    public string DomainName { get; set; } = string.Empty;
    public required bool LogEnabled { get; set; }
    public required A2ALoggerType LoggerType { get; set; }
}

public class TelegramOptions
{
    public string TelegramApiBaseUrl { get; set; } = string.Empty;
}
