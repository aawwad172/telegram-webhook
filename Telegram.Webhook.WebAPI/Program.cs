using Telegram.Webhook.Application;
using Telegram.Webhook.Domain;
using Telegram.Webhook.Infrastructure;
using Telegram.Webhook.Infrastructure.Persistence;
using Telegram.Webhook.WebAPI;
using Telegram.Webhook.WebAPI.Middlewares;
using Telegram.Webhook.WebAPI.Routes.HealthChecks;
using Telegram.Webhook.WebAPI.Routes.Webhook;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDomainServices()
                .AddApplicationServices()
                .AddInfrastructureServices(builder.Configuration)
                .AddWebAPIServices(builder.Configuration);

builder.Services.AddHealthChecks()
                .AddCheck<DbConnectionHealthCheck>("Database Connection");

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}


app.UseMiddleware<ExceptionHandlerMiddleware>();

# region Health Checks
// Add health check endpoint
app.MapGet("/health", HealthCheck.RegisterRoute)
    .WithName("HealthCheck")
    .WithOpenApi();
# endregion

RouteGroupBuilder api = app.MapGroup("/api");

#region Bot
api.MapPost("/bot/webhook/{PublicId}", ReceiveUpdate.RegisterRoute)
    .WithName("Telegram Updates Webhook")
    .WithTags("updates")
    .WithOpenApi();
#endregion

app.Run();
