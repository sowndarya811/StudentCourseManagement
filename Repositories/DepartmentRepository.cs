using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class DepartmentRepository
{
    public async Task AddAsync(Department department)
    {
        const string query = """
            INSERT INTO Departments
            (DepartmentName, Location, IsActive)
            VALUES
            (@DepartmentName, @Location, @IsActive)
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
        command.Parameters.AddWithValue("@Location", department.Location);
        command.Parameters.AddWithValue("@IsActive", department.IsActive);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<Department>> GetAllAsync()
    {
        const string query = """
            SELECT DepartmentId, DepartmentName, Location, IsActive
            FROM Departments
            ORDER BY DepartmentId
            """;

        var departments = new List<Department>();

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departments.Add(new Department
            {
                DepartmentId = reader.GetInt32(0),
                DepartmentName = reader.GetString(1),
                Location = reader.IsDBNull(2) ? "" : reader.GetString(2),
                IsActive = reader.GetBoolean(3)
            });
        }

        return departments;
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        const string query = """
            SELECT DepartmentId, DepartmentName, Location, IsActive
            FROM Departments
            WHERE DepartmentId = @DepartmentId
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@DepartmentId", id);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Department
            {
                DepartmentId = reader.GetInt32(0),
                DepartmentName = reader.GetString(1),
                Location = reader.IsDBNull(2) ? "" : reader.GetString(2),
                IsActive = reader.GetBoolean(3)
            };
        }

        return null;
    }

    public async Task UpdateAsync(Department department)
    {
        const string query = """
            UPDATE Departments
            SET DepartmentName = @DepartmentName,
                Location = @Location
            WHERE DepartmentId = @DepartmentId
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
        command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
        command.Parameters.AddWithValue("@Location", department.Location);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        const string query = """
            UPDATE Departments
            SET IsActive = 0
            WHERE DepartmentId = @DepartmentId
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@DepartmentId", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}