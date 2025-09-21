namespace Telegram.Webhook.Domain.Interfaces.Application;

public interface IAuthenticationService
{
    string Encrypt(string botKey, CancellationToken cancellationToken = default);
    string Decrypt(string encryptedBotKey, CancellationToken cancellationToken = default);
}
