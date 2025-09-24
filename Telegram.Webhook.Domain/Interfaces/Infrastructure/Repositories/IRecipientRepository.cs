namespace Telegram.Webhook.Domain.Interfaces.Infrastructure.Repositories;

public interface IRecipientRepository
{
    /// <summary>
    /// Add a new Telegram user chat mapping, that is, a user that has started a chat with the bot.
    /// </summary>
    Task AddAsync(
        int botId,
        string chatId,
        string? phoneNumber,
        long telegramUserId,
        string? username,
        string? firstName,
        bool isActive,
        CancellationToken ct);
}


