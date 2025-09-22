using System.Security.Cryptography;
using System.Text;
using Telegram.Webhook.Application.CQRS.Commands;
using Telegram.Webhook.Domain.Entities;

namespace Telegram.Webhook.Application.HelperServices;

public static class WebhookHelpers
{
    public static bool IsAuthorized(ReceiveUpdateCommand request, Bot bot)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(bot);

        // Fail-closed if no secret configured
        if (string.IsNullOrWhiteSpace(bot.WebhookSecret) || string.IsNullOrEmpty(request.SecretToken))
            return false;

        byte[] a = Encoding.UTF8.GetBytes(bot.WebhookSecret);
        byte[] b = Encoding.UTF8.GetBytes(request.SecretToken);
        if (a.Length != b.Length) return false;
        return CryptographicOperations.FixedTimeEquals(a, b);
    }
}
