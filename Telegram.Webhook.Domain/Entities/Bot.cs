namespace Telegram.Webhook.Domain.Entities;

public sealed class Bot
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required int CustomerId { get; init; }
    public required string PublicId { get; set; }
    public required string EncryptedBotKey { get; init; }
    public required string WebhookSecret { get; init; }
    public required string WebhookUrl { get; init; }
    public required bool IsActive { get; init; }
    public DateTime CreationDateTime { get; init; }
}
