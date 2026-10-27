using Microsoft.Data.SqlClient;

namespace StudentCourseManagement.Helpers;

public static class DatabaseHelper
{
    public const string ConnectionString =
        @"Server=.\SQLEXPRESS;Database=StudentCourseDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection CreateConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}