using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class EnrollmentRepository
{
    public async Task<int> EnrollAsync(int studentId, int courseId)
    {
        await using var connection = DatabaseHelper.CreateConnection();
        await connection.OpenAsync();

        await using var transaction =
            (SqlTransaction)await connection.BeginTransactionAsync();

        try
        {
            const string enrollmentSql = """
                INSERT INTO Enrollments
                (StudentId, CourseId, Status)
                VALUES
                (@StudentId, @CourseId, 'Enrolled');

                SELECT CAST(SCOPE_IDENTITY() AS INT);
                """;

            int enrollmentId;

            await using (var command =
                new SqlCommand(enrollmentSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@StudentId", studentId);
                command.Parameters.AddWithValue("@CourseId", courseId);

                enrollmentId = Convert.ToInt32(
                    await command.ExecuteScalarAsync());
            }

            const string auditSql = """
                INSERT INTO AuditLogs
                (EntityName, EntityId, Action, Details)
                VALUES
                (@EntityName, @EntityId, @Action, @Details);
                """;

            await using (var command =
                new SqlCommand(auditSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@EntityName", "Enrollment");
                command.Parameters.AddWithValue("@EntityId", enrollmentId);
                command.Parameters.AddWithValue("@Action", "ENROLLED");
                command.Parameters.AddWithValue(
                    "@Details",
                    $"Student {studentId} enrolled in Course {courseId}");

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();

            return enrollmentId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Enrollment>> GetAllAsync()
    {
        const string query = """
            SELECT EnrollmentId,
                   StudentId,
                   CourseId,
                   EnrolledAt,
                   Status
            FROM Enrollments
            ORDER BY EnrollmentId
            """;

        var enrollments = new List<Enrollment>();

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(new Enrollment
            {
                EnrollmentId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                CourseId = reader.GetInt32(2),
                EnrolledAt = reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return enrollments;
    }

    public async Task<List<Enrollment>> GetByStudentAsync(int studentId)
    {
        const string query = """
            SELECT EnrollmentId,
                   StudentId,
                   CourseId,
                   EnrolledAt,
                   Status
            FROM Enrollments
            WHERE StudentId = @StudentId
            ORDER BY EnrollmentId
            """;

        var enrollments = new List<Enrollment>();

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@StudentId", studentId);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(new Enrollment
            {
                EnrollmentId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                CourseId = reader.GetInt32(2),
                EnrolledAt = reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return enrollments;
    }

    public async Task<List<Enrollment>> GetByCourseAsync(int courseId)
    {
        const string query = """
            SELECT EnrollmentId,
                   StudentId,
                   CourseId,
                   EnrolledAt,
                   Status
            FROM Enrollments
            WHERE CourseId = @CourseId
            ORDER BY EnrollmentId
            """;

        var enrollments = new List<Enrollment>();

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@CourseId", courseId);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(new Enrollment
            {
                EnrollmentId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                CourseId = reader.GetInt32(2),
                EnrolledAt = reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return enrollments;
    }

    public async Task UpdateStatusAsync(
        int enrollmentId,
        string status)
    {
        await using var connection = DatabaseHelper.CreateConnection();
        await connection.OpenAsync();

        await using var transaction =
            (SqlTransaction)await connection.BeginTransactionAsync();

        try
        {
            const string updateSql = """
                UPDATE Enrollments
                SET Status = @Status
                WHERE EnrollmentId = @EnrollmentId
                """;

            await using (var command =
                new SqlCommand(updateSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue(
                    "@EnrollmentId",
                    enrollmentId);

                await command.ExecuteNonQueryAsync();
            }

            const string auditSql = """
                INSERT INTO AuditLogs
                (EntityName, EntityId, Action, Details)
                VALUES
                (@EntityName, @EntityId, @Action, @Details)
                """;

            await using (var command =
                new SqlCommand(auditSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@EntityName", "Enrollment");
                command.Parameters.AddWithValue(
                    "@EntityId",
                    enrollmentId);
                command.Parameters.AddWithValue(
                    "@Action",
                    "STATUS_CHANGED");
                command.Parameters.AddWithValue(
                    "@Details",
                    $"Enrollment status changed to {status}");

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}