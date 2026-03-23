using Domain.Entities;

namespace Domain.Contracts;

public interface IProjectTaskRepository
{
    Task<List<ProjectTask>> GetAllByProjectAsync(Guid projectId);
    Task<ProjectTask?> GetByIdAsync(Guid projectTaskId, Guid projectId);

    void Add(ProjectTask task);
    void Delete(ProjectTask task);

    Task SaveChangesAsync();
}
