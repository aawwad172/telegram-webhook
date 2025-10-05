using Microsoft.Data.SqlClient;
using System.Data;
using Telegram.Webhook.Domain.Entities;
using Telegram.Webhook.Domain.Interfaces.Infrastructure;
using Telegram.Webhook.Domain.Interfaces.Infrastructure.Repositories;

namespace Telegram.Webhook.Infrastructure.Persistence.Repositories;

public class BotRepository(IDbConnectionFactory dbConnectionFactory) : IBotRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<Bot?> GetByPublicIdAsync(string publicId, CancellationToken cancellationToken = default)
    {
        using IDbConnection conn = await _dbConnectionFactory.CreateOpenConnection();
        using SqlCommand cmd = (SqlCommand)conn.CreateCommand();

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "usp_GetBotByPublicId";
        cmd.CommandTimeout = 30;

        // NVARCHAR(128) per schema
        cmd.Parameters.Add(new SqlParameter("@PublicId", SqlDbType.NVarChar, 128) { Value = publicId });

        using SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        // map
        return new Bot
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
            EncryptedBotKey = reader.GetString(reader.GetOrdinal("EncryptedBotKey")),
            PublicId = reader.GetString(reader.GetOrdinal("PublicId")),
            WebhookSecret = reader.GetString(reader.GetOrdinal("WebhookSecret")),
            WebhookUrl = reader.GetString(reader.GetOrdinal("WebhookUrl")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
            CreationDateTime = reader.GetDateTime(reader.GetOrdinal("CreationDateTime"))
        };
    }
}
