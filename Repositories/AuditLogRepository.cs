using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class AuditLogRepository
{
    public async Task AddAsync(AuditLog auditLog)
    {
        const string query = """
            INSERT INTO AuditLogs
            (EntityName, EntityId, Action, Details)
            VALUES
            (@EntityName, @EntityId, @Action, @Details)
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue(
            "@EntityName",
            auditLog.EntityName);

        command.Parameters.AddWithValue(
            "@EntityId",
            auditLog.EntityId);

        command.Parameters.AddWithValue(
            "@Action",
            auditLog.Action);

        command.Parameters.AddWithValue(
            "@Details",
            auditLog.Details);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        const string query = """
            SELECT AuditId,
                   EntityName,
                   EntityId,
                   Action,
                   Details,
                   CreatedAt
            FROM AuditLogs
            ORDER BY AuditId DESC
            """;

        var logs = new List<AuditLog>();

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            logs.Add(new AuditLog
            {
                AuditId = reader.GetInt32(0),
                EntityName = reader.GetString(1),
                EntityId = reader.GetInt32(2),
                Action = reader.GetString(3),
                Details = reader.IsDBNull(4)
                    ? ""
                    : reader.GetString(4),
                CreatedAt = reader.GetDateTime(5)
            });
        }

        return logs;
    }
}