using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class AuditLogService
{
    private readonly AuditLogRepository repository;

    public AuditLogService(
        AuditLogRepository repository)
    {
        this.repository = repository;
    }

    public Task<List<AuditLog>> GetAllAsync()
    {
        return repository.GetAllAsync();
    }

    public async Task AddAsync(
        string entityName,
        int entityId,
        string action,
        string details)
    {
        var auditLog = new AuditLog
        {
            EntityName = entityName,
            EntityId = entityId,
            Action = action,
            Details = details,
            CreatedAt = DateTime.Now
        };

        await repository.AddAsync(auditLog);
    }
}