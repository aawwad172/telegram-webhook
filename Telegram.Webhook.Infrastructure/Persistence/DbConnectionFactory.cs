using System.Data;
using Telegram.Webhook.Domain.Interfaces.Infrastructure;
using Telegram.Webhook.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Telegram.API.Infrastructure.Persistence;

public class DbConnectionFactory(IOptionsMonitor<DbSettings> options) : IDbConnectionFactory
{
    private readonly IOptionsMonitor<DbSettings> _options = options;

    public async Task<IDbConnection> CreateOpenConnection()
    {
        SqlConnection conn = new(_options.CurrentValue.ConnectionString);
        await conn.OpenAsync().ConfigureAwait(false);
        return conn;
    }
}
