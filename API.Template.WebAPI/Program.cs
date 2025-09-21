using API.Template.Application;
using API.Template.Domain;
using API.Template.Infrastructure;
using API.Template.Infrastructure.Persistence;
using API.Template.WebAPI;
using API.Template.WebAPI.Middlewares;
using API.Template.WebAPI.Routes.HealthChecks;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDomainServices()
                .AddApplicationServices()
                .AddInfrastructureServices()
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

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();

# region Health Checks
// Add health check endpoint
app.MapGet("/health", HealthCheck.RegisterRoute)
    .WithName("HealthCheck")
    .WithOpenApi();
# endregion

app.Run();
