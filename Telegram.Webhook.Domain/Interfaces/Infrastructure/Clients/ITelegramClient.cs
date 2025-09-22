namespace Telegram.Webhook.Domain.Interfaces.Infrastructure.Clients;

public interface ITelegramClient
{
    Task<bool> SendTextAsync(
            string botToken,
            string chatId,
            string text,
            object? replyMarkup = null,
            CancellationToken ct = default);
}
