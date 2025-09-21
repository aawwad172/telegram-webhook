using API.Template.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace API.Template.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services here
        // Example: services.AddScoped<IMyApplicationService, MyApplicationService>();

        // Register Command and Query handlers
        //services.AddMediatR(cfg =>
        //{
        //    // Example: cfg.RegisterServicesFromAssembly(typeof(SendMessageCommandHandler).Assembly);
        //});

        MapsterConfiguration.RegisterMappings();

        return services;
    }
}
