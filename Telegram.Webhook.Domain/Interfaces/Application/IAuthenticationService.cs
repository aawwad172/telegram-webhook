namespace Telegram.Webhook.Domain.Interfaces.Application;

public interface IAuthenticationService
{
    string Encrypt(string botKey);
    string Decrypt(string encryptedBotKey);
}
