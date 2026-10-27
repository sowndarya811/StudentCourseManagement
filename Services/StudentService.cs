using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class StudentService
{
    private readonly StudentRepository repository;

    public StudentService(StudentRepository repository)
    {
        this.repository = repository;
    }

    public async Task AddAsync(
        string name,
        string email,
        int age,
        int departmentId)
    {
        ValidateStudent(name, email, age, departmentId);

        var student = new Student
        {
            Name = name.Trim(),
            Email = email.Trim(),
            Age = age,
            DepartmentId = departmentId,
            AdmissionDate = DateTime.Now,
            IsActive = true
        };

        await repository.AddAsync(student);
    }

    public Task<List<Student>> GetAllAsync()
    {
        return repository.GetAllAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException(
                "Invalid student ID.");

        return await repository.GetByIdAsync(id);
    }

    public async Task<List<Student>> SearchByNameAsync(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.");

        return await repository.SearchByNameAsync(
            name.Trim());
    }

    public async Task UpdateAsync(
        int id,
        string name,
        string email,
        int age,
        int departmentId)
    {
        ValidateStudent(name, email, age, departmentId);

        var existing = await repository.GetByIdAsync(id);

        if (existing is null)
            throw new ArgumentException(
                "Student not found.");

        existing.Name = name.Trim();
        existing.Email = email.Trim();
        existing.Age = age;
        existing.DepartmentId = departmentId;

        await repository.UpdateAsync(existing);
    }

    public async Task DeactivateAsync(int id)
    {
        var existing = await repository.GetByIdAsync(id);

        if (existing is null)
            throw new ArgumentException(
                "Student not found.");

        await repository.DeactivateAsync(id);
    }

    private static void ValidateStudent(
        string name,
        string email,
        int age,
        int departmentId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Student name is required.");

        if (string.IsNullOrWhiteSpace(email) ||
            !email.Contains('@'))
            throw new ArgumentException(
                "Valid email is required.");

        if (age < 17 || age > 60)
            throw new ArgumentException(
                "Age must be between 17 and 60.");

        if (departmentId <= 0)
            throw new ArgumentException(
                "Invalid department ID.");
    }
}