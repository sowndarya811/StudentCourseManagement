using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class DepartmentService
{
    private readonly DepartmentRepository repository;

    public DepartmentService(DepartmentRepository repository)
    {
        this.repository = repository;
    }

    public async Task AddAsync(
        string name,
        string location)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Department name is required.");

        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException(
                "Location is required.");

        var department = new Department
        {
            DepartmentName = name.Trim(),
            Location = location.Trim(),
            IsActive = true
        };

        await repository.AddAsync(department);
    }

    public Task<List<Department>> GetAllAsync()
    {
        return repository.GetAllAsync();
    }

    public Task<Department?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException(
                "Invalid department ID.");

        return repository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(
        int id,
        string name,
        string location)
    {
        if (id <= 0)
            throw new ArgumentException(
                "Invalid department ID.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Department name is required.");

        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException(
                "Location is required.");

        var existing = await repository.GetByIdAsync(id);

        if (existing is null)
            throw new ArgumentException(
                "Department not found.");

        existing.DepartmentName = name.Trim();
        existing.Location = location.Trim();

        await repository.UpdateAsync(existing);
    }

    public async Task DeactivateAsync(int id)
    {
        var existing = await repository.GetByIdAsync(id);

        if (existing is null)
            throw new ArgumentException(
                "Department not found.");

        await repository.DeactivateAsync(id);
    }
}