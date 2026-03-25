using Domain.Entities;  

namespace Domain.Contracts;

public interface IProjectTaskRepository
{
    Task<List<ProjectTask>> GetAllByProjectAsync(Guid projectId, Guid userId);
    Task<ProjectTask?> GetByIdAsync(Guid taskId, Guid projectId, Guid userId);

    void Add(ProjectTask task);
    void Delete(ProjectTask task);

    Task SaveChangesAsync();
}
