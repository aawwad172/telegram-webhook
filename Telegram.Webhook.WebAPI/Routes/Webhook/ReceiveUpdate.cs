using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Telegram.Webhook.Application.CQRS.Commands;
using Telegram.Webhook.Domain.Entities;
using Telegram.Webhook.Domain.Exceptions;
using Telegram.Webhook.WebAPI.Models;

namespace Telegram.Webhook.WebAPI.Routes.Webhook;

public class ReceiveUpdate
{
    public static async Task<IResult> RegisterRoute(
        HttpContext httpContext,
        [FromServices] IMediator mediator,
        [FromServices] IValidator<ReceiveUpdateCommand> validator,
        CancellationToken cancellationToken = default)
    {

        // 1) Extract route + header (don’t validate here, just read)
        string? publicId = httpContext.Request.RouteValues.TryGetValue("PublicId", out object? pidObj)
            ? pidObj?.ToString()
            : null;

        httpContext.Request.Headers.TryGetValue("X-Telegram-Bot-Api-Secret-Token", out Microsoft.Extensions.Primitives.StringValues secretVals);
        string? secretToken = secretVals.Count > 0 ? secretVals[0] : null;

        // 2) Deserialize Telegram update with endpoint-local snake_case
        TelegramUpdate? update;
        try
        {
            JsonSerializerOptions opts = BuildSnakeCaseJsonOptions();
            update = await JsonSerializer.DeserializeAsync<TelegramUpdate>(httpContext.Request.Body, opts, cancellationToken);
        }
        catch (JsonException)
        {
            // Malformed JSON -> 400
            throw new BadRequestException("Invalid JSON payload.");
        }

        // 3) Build command and validate (all input checks live here)
        ReceiveUpdateCommand command = new(PublicId: publicId, SecretToken: secretToken, Update: update);

        ValidationResult validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            List<string> errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            // Your middleware maps this to 400 with a neat payload
            throw new CustomValidationException("Validation failed", errors);
        }

        // 4) Do the work
        ReceiveUpdateCommandResult result = await mediator.Send(command, cancellationToken);
        return Results.Ok(ApiResponse<ReceiveUpdateCommandResult>.SuccessResponse(result));
    }

    // Local, endpoint-scoped snake_case options (no global side-effects)
    private static JsonSerializerOptions BuildSnakeCaseJsonOptions()
    {
        return new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true
        };
    }
}
