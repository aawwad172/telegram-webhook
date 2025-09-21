
using A2ASMS.Utility.Data;
using Telegram.Webhook.Domain.Interfaces.Application;

namespace Telegram.Webhook.Application.HelperServices;

public class AuthenticationService : IAuthenticationService
{
    public string Decrypt(string encryptedBotKey, CancellationToken cancellationToken = default)
        => EncrLib.Decrypt(encryptedBotKey);

    public string Encrypt(string botKey, CancellationToken cancellationToken = default)
        => EncrLib.Encrypt(botKey);
}
