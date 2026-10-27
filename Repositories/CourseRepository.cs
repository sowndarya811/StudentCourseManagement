using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class CourseRepository
{
    public async Task AddAsync(Course course)
    {
        const string query = """
            INSERT INTO Courses
            (CourseName, DurationMonths, Fee, DepartmentId, IsActive)
            VALUES
            (@CourseName, @DurationMonths, @Fee, @DepartmentId, @IsActive)
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@CourseName", course.CourseName);
        command.Parameters.AddWithValue("@DurationMonths", course.DurationMonths);
        command.Parameters.AddWithValue("@Fee", course.Fee);
        command.Parameters.AddWithValue("@DepartmentId", course.DepartmentId);
        command.Parameters.AddWithValue("@IsActive", course.IsActive);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<Course>> GetAllAsync()
    {
        const string query = """
            SELECT CourseId, CourseName, DurationMonths,
                   Fee, DepartmentId, IsActive
            FROM Courses
            ORDER BY CourseId
            """;

        var courses = new List<Course>();

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courses.Add(new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return courses;
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        const string query = """
            SELECT CourseId, CourseName, DurationMonths,
                   Fee, DepartmentId, IsActive
            FROM Courses
            WHERE CourseId = @CourseId
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@CourseId", id);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            };
        }

        return null;
    }

    public async Task<List<Course>> SearchByNameAsync(string name)
    {
        const string query = """
            SELECT CourseId, CourseName, DurationMonths,
                   Fee, DepartmentId, IsActive
            FROM Courses
            WHERE CourseName LIKE @CourseName
            ORDER BY CourseId
            """;

        var courses = new List<Course>();

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@CourseName", $"%{name}%");

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courses.Add(new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return courses;
    }

    public async Task UpdateAsync(Course course)
    {
        const string query = """
            UPDATE Courses
            SET CourseName = @CourseName,
                DurationMonths = @DurationMonths,
                Fee = @Fee,
                DepartmentId = @DepartmentId
            WHERE CourseId = @CourseId
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@CourseId", course.CourseId);
        command.Parameters.AddWithValue("@CourseName", course.CourseName);
        command.Parameters.AddWithValue("@DurationMonths", course.DurationMonths);
        command.Parameters.AddWithValue("@Fee", course.Fee);
        command.Parameters.AddWithValue("@DepartmentId", course.DepartmentId);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        const string query = """
            UPDATE Courses
            SET IsActive = 0
            WHERE CourseId = @CourseId
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@CourseId", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}