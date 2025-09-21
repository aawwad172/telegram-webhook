using Telegram.Webhook.Application.CQRS.Commands;
using Telegram.Webhook.Domain.Entities;

namespace Telegram.Webhook.Application.HelperServices;

public class WebhookHelpers
{
    public static bool IsAuthorized(ReceiveUpdateCommand request, Bot bot)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(bot);

        // Fail-closed if no secret configured
        if (string.IsNullOrWhiteSpace(bot.WebhookSecret))
            return false;

        return string.Equals(bot.WebhookSecret, request.SecretToken, StringComparison.Ordinal);
    }
}
