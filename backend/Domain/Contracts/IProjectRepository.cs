using Domain.Entities;

namespace Domain.Contracts;

public interface IProjectRepository
{
    public Task<List<Project>> GetAllByUserAsync(Guid userId);
    public Task<Project?> GetByIdAsync(Guid projectId, Guid userId);

    public void Add(Project task);
    public void Delete(Project task);

    public Task SaveChangesAsync();
}
