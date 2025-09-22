using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Telegram.Webhook.Domain.Entities;
using Telegram.Webhook.Domain.Exceptions;
using Telegram.Webhook.Domain.Interfaces.Infrastructure.Clients;

namespace Telegram.Webhook.Infrastructure.Clients;

public class TelegramClient : ITelegramClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json; // snake_case in/out

    public TelegramClient(HttpClient httpClient)
    {
        _http = httpClient;
        _json = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
    public async Task<bool> SendTextAsync(
        string botToken,
        string chatId,
        string text,
        object? replyMarkup = null,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(botToken)) throw new ArgumentException("Required.", nameof(botToken));
        if (string.IsNullOrWhiteSpace(chatId)) throw new ArgumentException("Required.", nameof(chatId));
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Required.", nameof(text));

        var payload = new
        {
            chat_id = chatId,
            text,
            reply_markup = replyMarkup
        };

        TelegramResponse<JsonElement> resp =
            await PostJsonAsync<JsonElement>($"/bot{botToken}/sendMessage", payload, ct);
        return resp.Ok;
    }

    private async Task<TelegramResponse<T>> PostJsonAsync<T>(string path, object payload, CancellationToken ct)
    {
        using StringContent content = new StringContent(JsonSerializer.Serialize(payload, _json), Encoding.UTF8, "application/json");
        using HttpResponseMessage res = await _http.PostAsync(path, content, ct);
        string body = await res.Content.ReadAsStringAsync(ct);

        TelegramResponse<T>? parsed;
        try
        {
            parsed = JsonSerializer.Deserialize<TelegramResponse<T>>(body, _json);
        }
        catch (JsonException ex)
        {
            throw new TelegramApiException($"Telegram JSON parse error: {ex.Message}");
        }

        if (parsed is null)
            throw new TelegramApiException("Empty response from Telegram.");

        if (!parsed.Ok)
            throw new TelegramApiException($"Telegram Error request: {parsed.ErrorCode} {parsed.Description}");

        return parsed;
    }
}
