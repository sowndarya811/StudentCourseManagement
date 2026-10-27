using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class EnrollmentService
{
    private readonly EnrollmentRepository repository;

    public EnrollmentService(
        EnrollmentRepository repository)
    {
        this.repository = repository;
    }

    public async Task<int> EnrollAsync(
        int studentId,
        int courseId)
    {
        if (studentId <= 0)
            throw new ArgumentException(
                "Invalid student ID.");

        if (courseId <= 0)
            throw new ArgumentException(
                "Invalid course ID.");

        return await repository.EnrollAsync(
            studentId,
            courseId);
    }

    public Task<List<Enrollment>> GetAllAsync()
    {
        return repository.GetAllAsync();
    }

    public async Task<List<Enrollment>> GetByStudentAsync(
        int studentId)
    {
        if (studentId <= 0)
            throw new ArgumentException(
                "Invalid student ID.");

        return await repository.GetByStudentAsync(
            studentId);
    }

    public async Task<List<Enrollment>> GetByCourseAsync(
        int courseId)
    {
        if (courseId <= 0)
            throw new ArgumentException(
                "Invalid course ID.");

        return await repository.GetByCourseAsync(
            courseId);
    }

    public async Task UpdateStatusAsync(
        int enrollmentId,
        string status)
    {
        if (enrollmentId <= 0)
            throw new ArgumentException(
                "Invalid enrollment ID.");

        if (status != "Enrolled" &&
            status != "Completed" &&
            status != "Cancelled")
        {
            throw new ArgumentException(
                "Status must be Enrolled, Completed or Cancelled.");
        }

        await repository.UpdateStatusAsync(
            enrollmentId,
            status);
    }
}