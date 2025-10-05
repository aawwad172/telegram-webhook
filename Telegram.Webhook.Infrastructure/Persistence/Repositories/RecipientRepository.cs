using Microsoft.Data.SqlClient;
using System.Data;
using Telegram.Webhook.Domain.Interfaces.Infrastructure;
using Telegram.Webhook.Domain.Interfaces.Infrastructure.Repositories;

namespace Telegram.Webhook.Infrastructure.Persistence.Repositories;

public class RecipientRepository(IDbConnectionFactory dbFactory) : IRecipientRepository
{
    private readonly IDbConnectionFactory _dbFactory = dbFactory;
    public async Task AddAsync(
        int botId,
        string chatId,
        string? phoneNumber,
        long telegramUserId,
        string? username,
        string? firstName,
        bool isActive,
        CancellationToken ct)
    {
        using IDbConnection conn = await _dbFactory.CreateOpenConnection();
        using SqlCommand cmd = (SqlCommand)conn.CreateCommand();

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "usp_Recipient_Upsert";
        cmd.CommandTimeout = 30;

        cmd.Parameters.Add(new SqlParameter("@BotId", SqlDbType.Int) { Value = botId });
        cmd.Parameters.Add(new SqlParameter("@ChatId", SqlDbType.NVarChar, 50) { Value = chatId });

        cmd.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.NVarChar, 32)
        { Value = phoneNumber is not null ? phoneNumber : DBNull.Value });

        cmd.Parameters.Add(new SqlParameter("@TelegramUserId", SqlDbType.BigInt)
        { Value = telegramUserId });

        cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 64)
        { Value = username is not null ? username : DBNull.Value });

        cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 64)
        { Value = firstName is not null ? firstName : DBNull.Value });

        cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = isActive });

        _ = await cmd.ExecuteNonQueryAsync(ct);
    }
}
