using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Telegram.Webhook.WebAPI.Routes.HealthChecks;

public class HealthCheck
{
    public static async Task<IResult> RegisterRoute(
        HealthCheckService request)
    {
        HealthReport report = await request.CheckHealthAsync();
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        };

        return Results.Ok(response);
    }
}
