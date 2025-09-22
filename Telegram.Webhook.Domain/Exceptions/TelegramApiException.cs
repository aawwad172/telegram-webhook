namespace Telegram.Webhook.Domain.Exceptions;

public class TelegramApiException : Exception
{
    public int? ErrorCode { get; }
    public string? Description { get; }

    public TelegramApiException(string? message) : base(message)
    {
    }

    public TelegramApiException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public TelegramApiException(int? errorCode, string? description, Exception? innerException = null)
        : base(description, innerException)
    {
        ErrorCode = errorCode;
        Description = description;
    }
}