namespace Telegram.Webhook.Domain.Entities;

public sealed class Recipient
{
    public required int BotId { get; set; }
    public required string ChatId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public DateTime CreationDateTime { get; init; }
    public DateTime LastSeenDateTime { get; init; }
    public bool IsActive { get; init; }
}
