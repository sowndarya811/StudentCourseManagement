using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class CourseService
{
    private readonly CourseRepository repository;

    public CourseService(CourseRepository repository)
    {
        this.repository = repository;
    }

    public async Task AddAsync(
        string name,
        int durationMonths,
        decimal fee,
        int departmentId)
    {
        ValidateCourse(
            name,
            durationMonths,
            fee,
            departmentId);

        var course = new Course
        {
            CourseName = name.Trim(),
            DurationMonths = durationMonths,
            Fee = fee,
            DepartmentId = departmentId,
            IsActive = true
        };

        await repository.AddAsync(course);
    }

    public Task<List<Course>> GetAllAsync()
    {
        return repository.GetAllAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException(
                "Invalid course ID.");

        return await repository.GetByIdAsync(id);
    }

    public async Task<List<Course>> SearchByNameAsync(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Course name is required.");

        return await repository.SearchByNameAsync(
            name.Trim());
    }

    public async Task UpdateAsync(
        int id,
        string name,
        int durationMonths,
        decimal fee,
        int departmentId)
    {
        ValidateCourse(
            name,
            durationMonths,
            fee,
            departmentId);

        var existing = await repository.GetByIdAsync(id);

        if (existing is null)
            throw new ArgumentException(
                "Course not found.");

        existing.CourseName = name.Trim();
        existing.DurationMonths = durationMonths;
        existing.Fee = fee;
        existing.DepartmentId = departmentId;

        await repository.UpdateAsync(existing);
    }

    public async Task DeactivateAsync(int id)
    {
        var existing = await repository.GetByIdAsync(id);

        if (existing is null)
            throw new ArgumentException(
                "Course not found.");

        await repository.DeactivateAsync(id);
    }

    private static void ValidateCourse(
        string name,
        int durationMonths,
        decimal fee,
        int departmentId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Course name is required.");

        if (durationMonths <= 0)
            throw new ArgumentException(
                "Duration must be greater than 0.");

        if (fee < 0)
            throw new ArgumentException(
                "Fee cannot be negative.");

        if (departmentId <= 0)
            throw new ArgumentException(
                "Invalid department ID.");
    }
}