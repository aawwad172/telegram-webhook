using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Webhook.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
        => services;
}
