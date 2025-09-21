using FluentValidation;
using Telegram.Webhook.Application.CQRS.Commands;

namespace Telegram.Webhook.WebAPI.Validators.Commands;

public class ReceiveUpdateCommandValidator : AbstractValidator<ReceiveUpdateCommand>
{
    public ReceiveUpdateCommandValidator()
    {
        RuleFor(x => x.PublicId)
            .NotEmpty().WithMessage("PublicId is required.")
            .Length(32).WithMessage("PublicId must be a 32-char hex string."); // adjust if GUID with dashes

        RuleFor(x => x.SecretToken)
            .NotEmpty().WithMessage("SecretToken is required.");

        RuleFor(x => x.Update)
            .NotNull().WithMessage("Update is required.");

        // Optional: require minimum message structure for your flow
        When(x => x.Update is not null, () =>
        {
            RuleFor(x => x.Update)
                .NotNull().WithMessage("Update is required.")
                .Must(u => u.Message != null || u.CallbackQuery != null)
                .WithMessage("Either Message or CallbackQuery is required.");

            RuleFor(x => x.Update!.Message)
                .NotNull().WithMessage("Update.message is required.");

            RuleFor(x => x.Update!.Message!.Chat)
                .NotNull().WithMessage("Update.message.chat is required.");
        });
    }
}
