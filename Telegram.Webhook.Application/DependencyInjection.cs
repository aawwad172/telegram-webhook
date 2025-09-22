using Microsoft.Extensions.DependencyInjection;
using Telegram.Webhook.Application.CQRS.CommandHandlers;
using Telegram.Webhook.Application.HelperServices;
using Telegram.Webhook.Application.Utilities;
using Telegram.Webhook.Domain.Interfaces.Application;

namespace Telegram.Webhook.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services here
        // Example: services.AddScoped<IMyApplicationService, MyApplicationService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        // Register Command and Query handlers
        services.AddMediatR(cfg =>
        {
            // Example: cfg.RegisterServicesFromAssembly(typeof(SendMessageCommandHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(ReceiveUpdateCommandHandler).Assembly);

        });

        MapsterConfiguration.RegisterMappings();

        return services;
    }
}
