using MediatR;
using Telegram.Webhook.Domain.Entities;

namespace Telegram.Webhook.Application.CQRS.Commands;

public record ReceiveUpdateCommand(string? PublicId, string? SecretToken, TelegramUpdate? Update) : IRequest<ReceiveUpdateCommandResult>;

public sealed record ReceiveUpdateCommandResult();