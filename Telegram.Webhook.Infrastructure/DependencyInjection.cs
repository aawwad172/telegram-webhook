using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Webhook.Domain.Interfaces.Infrastructure;
using Telegram.Webhook.Domain.Interfaces.Infrastructure.Clients;
using Telegram.Webhook.Domain.Interfaces.Infrastructure.Repositories;
using Telegram.Webhook.Domain.Settings;
using Telegram.Webhook.Infrastructure.Clients;
using Telegram.Webhook.Infrastructure.Persistence;
using Telegram.Webhook.Infrastructure.Persistence.Repositories;

namespace Telegram.Webhook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register your infrastructure services here
        // Example: services.AddSingleton<IMyService, MyService>();

        // Register your infrastructure services
        services.AddTransient<DbConnectionHealthCheck>();
        services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();

        services.AddTransient<DbConnectionHealthCheck>();
        services.AddTransient<IBotRepository, BotRepository>();
        services.AddTransient<IRecipientRepository, RecipientRepository>();

        services.AddOptions<TelegramOptions>()
        .Bind(configuration.GetRequiredSection(nameof(TelegramOptions)))
        .Validate(o => Uri.TryCreate(o.TelegramApiBaseUrl, UriKind.Absolute, out _), "TelegramApiBaseUrl must be a valid absolute URI.")
        .ValidateOnStart();

        services.AddHttpClient<ITelegramClient, TelegramClient>((serviceProvider, client) =>
        {
            TelegramOptions opts = serviceProvider.GetRequiredService<IOptionsMonitor<TelegramOptions>>().CurrentValue;

            if (opts.TelegramApiBaseUrl is null)
            {
                throw new ArgumentNullException("Telegram Api Base URL is empty");
            }

            client.BaseAddress = new Uri(opts!.TelegramApiBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}
