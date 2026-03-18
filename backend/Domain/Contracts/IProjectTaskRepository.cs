using Domain.Entities;

namespace Domain.Contracts;

public interface IProjectTaskRepository
{
    public Task<List<ProjectTask>> GetAllByProjectAsync(Guid projectId);
    public Task<ProjectTask?> GetByProjectAsync(Guid projectTaskId, Guid projectId);

    public Task AddAsync(ProjectTask task);
    public Task DeleteAsync(ProjectTask task);

    public Task SaveChangesAsync();
}
