using Telegram.Webhook.Domain.Entities;

namespace Telegram.Webhook.Domain.Interfaces.Infrastructure.Repositories;

public interface IBotRepository
{
    Task<Bot?> GetByPublicIdAsync(string publicId, CancellationToken cancellationToken = default);
}

