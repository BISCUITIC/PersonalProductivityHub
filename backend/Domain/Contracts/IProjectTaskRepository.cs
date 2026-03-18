using Domain.Entities;

namespace Domain.Contracts;

public interface IProjectTaskRepository
{
    public Task<List<ProjectTask>> GetAllByProjectAsync(Guid projectId);
    public Task<ProjectTask?> GetByIdAsync(Guid projectTaskId, Guid projectId);

    public void Add(ProjectTask task);
    public void Delete(ProjectTask task);

    public Task SaveChangesAsync();
}
