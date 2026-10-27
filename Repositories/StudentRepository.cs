using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class StudentRepository
{
    public async Task AddAsync(Student student)
    {
        const string query = """
            INSERT INTO Students
            (Name, Email, Age, DepartmentId, AdmissionDate, IsActive)
            VALUES
            (@Name, @Email, @Age, @DepartmentId, @AdmissionDate, @IsActive)
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Name", student.Name);
        command.Parameters.AddWithValue("@Email", student.Email);
        command.Parameters.AddWithValue("@Age", student.Age);
        command.Parameters.AddWithValue("@DepartmentId", student.DepartmentId);
        command.Parameters.AddWithValue("@AdmissionDate", student.AdmissionDate);
        command.Parameters.AddWithValue("@IsActive", student.IsActive);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<Student>> GetAllAsync()
    {
        const string query = """
            SELECT StudentId, Name, Email, Age,
                   DepartmentId, AdmissionDate, IsActive
            FROM Students
            ORDER BY StudentId
            """;

        var students = new List<Student>();

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            students.Add(new Student
            {
                StudentId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4),
                AdmissionDate = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            });
        }

        return students;
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        const string query = """
            SELECT StudentId, Name, Email, Age,
                   DepartmentId, AdmissionDate, IsActive
            FROM Students
            WHERE StudentId = @StudentId
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@StudentId", id);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Student
            {
                StudentId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4),
                AdmissionDate = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            };
        }

        return null;
    }

    public async Task<List<Student>> SearchByNameAsync(string name)
    {
        const string query = """
            SELECT StudentId, Name, Email, Age,
                   DepartmentId, AdmissionDate, IsActive
            FROM Students
            WHERE Name LIKE @Name
            ORDER BY StudentId
            """;

        var students = new List<Student>();

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Name", $"%{name}%");

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            students.Add(new Student
            {
                StudentId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4),
                AdmissionDate = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            });
        }

        return students;
    }

    public async Task UpdateAsync(Student student)
    {
        const string query = """
            UPDATE Students
            SET Name = @Name,
                Email = @Email,
                Age = @Age,
                DepartmentId = @DepartmentId
            WHERE StudentId = @StudentId
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@StudentId", student.StudentId);
        command.Parameters.AddWithValue("@Name", student.Name);
        command.Parameters.AddWithValue("@Email", student.Email);
        command.Parameters.AddWithValue("@Age", student.Age);
        command.Parameters.AddWithValue("@DepartmentId", student.DepartmentId);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        const string query = """
            UPDATE Students
            SET IsActive = 0
            WHERE StudentId = @StudentId
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@StudentId", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}