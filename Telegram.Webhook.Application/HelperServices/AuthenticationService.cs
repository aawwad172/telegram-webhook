
using A2ASMS.Utility.Data;
using Telegram.Webhook.Domain.Interfaces.Application;

namespace Telegram.Webhook.Application.HelperServices;

public class AuthenticationService : IAuthenticationService
{
    public string Decrypt(string encryptedBotKey)
        => EncrLib.Decrypt(encryptedBotKey);

    public string Encrypt(string botKey)
        => EncrLib.Encrypt(botKey);
}
