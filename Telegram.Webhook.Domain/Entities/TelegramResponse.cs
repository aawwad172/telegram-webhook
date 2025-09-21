namespace Telegram.Webhook.Domain.Entities;

public class TelegramResponse<T>
{
    public bool Ok { get; set; }

    public T? Result { get; set; }

    public int? ErrorCode { get; set; }

    public string? Description { get; set; }
}
