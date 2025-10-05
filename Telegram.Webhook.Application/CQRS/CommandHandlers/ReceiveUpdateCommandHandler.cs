using MediatR;
using Telegram.Webhook.Application.CQRS.Commands;
using Telegram.Webhook.Application.HelperServices;
using Telegram.Webhook.Domain.Entities;
using Telegram.Webhook.Domain.Exceptions;
using Telegram.Webhook.Domain.Interfaces.Application;
using Telegram.Webhook.Domain.Interfaces.Infrastructure.Clients;
using Telegram.Webhook.Domain.Interfaces.Infrastructure.Repositories;

namespace Telegram.Webhook.Application.CQRS.CommandHandlers;

public class ReceiveUpdateCommandHandler(
    IBotRepository botRepository,
    IRecipientRepository recipientRepository,
    ITelegramClient telegramClient,
    IAuthenticationService authenticationService)
    : IRequestHandler<ReceiveUpdateCommand, ReceiveUpdateCommandResult>
{
    private readonly IBotRepository _botRepository = botRepository;
    private readonly IRecipientRepository _recipientRepository = recipientRepository;
    private readonly ITelegramClient _telegramClient = telegramClient;
    private readonly IAuthenticationService _authenticationService = authenticationService;
    public async Task<ReceiveUpdateCommandResult> Handle(ReceiveUpdateCommand request, CancellationToken cancellationToken)
    {
        // 1) Resolve bot by PublicId
        Bot? bot = await _botRepository.GetByPublicIdAsync(request.PublicId!, cancellationToken)
                  ?? throw new NotFoundException("Bot not found for publicId");

        // 2) Validate secret if configured
        if (!WebhookHelpers.IsAuthorized(request, bot))
            throw new UnauthorizedException("Invalid Telegram webhook secret.");

        if (request.Update is null)
            return new();

        // 3) We only care about private /start messages
        TelegramUpdateMessage? msg = request.Update.Message;
        if (msg is null || msg.Chat is null)
            return new();

        bool isPrivate = string.Equals(msg.Chat.Type, "private", StringComparison.OrdinalIgnoreCase);
        if (!isPrivate) return new();

        string? text = msg.Text?.Trim();
        bool isStart = text?.StartsWith("/start", StringComparison.OrdinalIgnoreCase) == true;

        string chatId = msg.Chat.Id.ToString();
        long tgUserId = msg.From?.Id ?? 0;
        if (tgUserId == 0) return new();

        // 1) CONTACT MESSAGE → save phone, confirm, return
        if (msg.Contact is not null)
        {
            // (optional) ensure the shared contact belongs to the sender
            if (msg.Contact.UserId is long contactUserId && contactUserId != 0 && contactUserId != tgUserId)
                return new(); // ignore contacts of other users

            string? phone = msg.Contact.PhoneNumber?.Trim();
            if (!string.IsNullOrWhiteSpace(phone) && CommandSanitizerHelpers.TryNormalizePhoneNumber(phone!, out string? normalized))
                phone = normalized;

            await _recipientRepository.AddAsync(
                botId: bot.Id,
                chatId: chatId,
                phoneNumber: phone,
                telegramUserId: tgUserId,
                username: msg.From?.Username?.Trim(),
                firstName: msg.From?.FirstName?.Trim(),
                isActive: true,
                ct: cancellationToken
            );


            string token = _authenticationService.Decrypt(bot.EncryptedBotKey);
            await _telegramClient.SendTextAsync(
               token,
               chatId,
               "✅ You’re all set! You will start receiving updates.",
               replyMarkup: new
               {
                   remove_keyboard = true
                   // selective = true // <- only if you need to target a specific user in groups
               },
               ct: cancellationToken
            );

            return new();
        }

        // 2) /start → create/update row, prompt for phone
        if (isStart)
        {
            await _recipientRepository.AddAsync(
                botId: bot.Id,
                chatId: chatId,
                phoneNumber: null,                 // none yet
                telegramUserId: tgUserId,
                username: msg.From?.Username?.Trim(),
                firstName: msg.From?.FirstName?.Trim(),
                isActive: true,
                ct: cancellationToken
            );


            string token = _authenticationService.Decrypt(bot.EncryptedBotKey);
            // ✅ Send ReplyKeyboardMarkup with a contact button
            await _telegramClient.SendTextAsync(
                token,
                chatId,
                "Please share your phone number to complete your subscription.",
                replyMarkup: new
                {
                    keyboard = new[]
                    {
                        new[] { new { text = "Get Started", request_contact = true } }
                    },
                    resize_keyboard = true,
                    one_time_keyboard = true // hides on press in many clients, but we still remove explicitly after contact
                },
                ct: cancellationToken
            );
            return new();
        }

        // ignore other messages
        return new();
    }
}
