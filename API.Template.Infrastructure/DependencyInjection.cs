using API.Template.Domain.Interfaces.Infrastructure;
using API.Template.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.API.Infrastructure.Persistence;

namespace API.Template.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Register your infrastructure services here
        // Example: services.AddSingleton<IMyService, MyService>();

        // Register your infrastructure services
        services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();

        services.AddTransient<DbConnectionHealthCheck>();

        return services;
    }
}
