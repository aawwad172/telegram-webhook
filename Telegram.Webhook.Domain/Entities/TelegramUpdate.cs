namespace Telegram.Webhook.Domain.Entities;

public class TelegramUpdate
{
    public long UpdateId { get; set; }
    public TelegramUpdateMessage? Message { get; set; }
    public TelegramCallbackQuery? CallbackQuery { get; set; }
}

public class TelegramUpdateMessage
{
    public long MessageId { get; set; }
    public TelegramUser? From { get; set; }
    public long Date { get; set; }
    public string? Text { get; set; }
    public TelegramChat? Chat { get; set; }
    public TelegramContact? Contact { get; set; } // for phone sharing
}

public class TelegramUser
{
    public long Id { get; set; }
    public bool IsBot { get; set; }
    public string? FirstName { get; set; }
    public string? Username { get; set; }
}

public class TelegramChat
{
    public long Id { get; set; }
    public string? Type { get; set; }
}

public class TelegramCallbackQuery
{
    public string? Id { get; set; }
    public TelegramUser? From { get; set; }
    public string? Data { get; set; }
}

public class TelegramContact
{
    public string? PhoneNumber { get; set; }
    public long UserId { get; set; }
    public string? FirstName { get; set; }
}
