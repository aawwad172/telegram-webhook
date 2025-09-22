using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Telegram.Webhook.WebAPI.Interfaces;

/// <summary>
/// This interface is used to define a route for a parameterized query so we can validate the query params.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
public interface IParameterizedQueryRoute<TQuery> where TQuery : notnull
{
    static abstract Task<IResult> RegisterRoute(
    [AsParameters] TQuery query,
    [FromServices] IMediator mediator,
    [FromServices] IValidator<TQuery> validator);
}
