using A2ASMS.Utility.Logger;
using API.Template.Domain.Settings;

namespace API.Template.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));
        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

        AppSettings? appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()
                    ?? throw new InvalidOperationException("AppSettings section is missing.");

        appSettings.LoggerType = (A2ALoggerType)Enum.Parse(typeof(A2ALoggerType), configuration["AppSettings:LoggerType"]!, true);

        A2ALoggerConfig.LogPath = appSettings!.LogPath;
        A2ALoggerConfig.FlushInterval = appSettings.LogFlushInterval;
        A2ALoggerConfig.LogEnabled = appSettings.LogEnabled;
        A2ALoggerConfig.LoggerType = appSettings.LoggerType;

        services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        // Don't forget to register the Validators
        // services.AddTransient<IValidator<SendMessageCommand>, SendMessageCommandValidator>();

        return services;
    }
}
