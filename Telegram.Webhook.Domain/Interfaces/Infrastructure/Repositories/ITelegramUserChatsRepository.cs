namespace Telegram.Webhook.Domain.Interfaces.Infrastructure.Repositories;

public interface ITelegramUserChatsRepository
{
    /// <summary>
    /// Insert a new chat or update it if it already exists.
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


