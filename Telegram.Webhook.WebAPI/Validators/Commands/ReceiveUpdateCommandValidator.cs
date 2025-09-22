using FluentValidation;
using Telegram.Webhook.Application.CQRS.Commands;

namespace Telegram.Webhook.WebAPI.Validators.Commands;

public class ReceiveUpdateCommandValidator : AbstractValidator<ReceiveUpdateCommand>
{
    public ReceiveUpdateCommandValidator()
    {
        RuleFor(x => x.PublicId)
            .NotEmpty().WithMessage("PublicId is required.")
            .Length(32).WithMessage("PublicId must be 32 chars.")
            .Matches("^[0-9a-fA-F]{32}$").WithMessage("PublicId must be a hex string.");

        RuleFor(x => x.SecretToken)
            .NotEmpty().WithMessage("SecretToken is required.");

        RuleFor(x => x.Update)
            .NotNull().WithMessage("Update is required.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Update!)
                    .Must(u => u.Message != null)
                    .WithMessage("Message required.");

                When(x => x.Update!.Message is not null, () =>
                {
                    RuleFor(x => x.Update!.Message!.Chat)
                        .NotNull().WithMessage("Update.message.chat is required.");
                });
            });
    }
}
