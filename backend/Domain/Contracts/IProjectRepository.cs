using Domain.Entities;

namespace Domain.Contracts;

public interface IProjectRepository
{
    public Task<List<Project>> GetAllByUserAsync(Guid userId);
    public Task<Project?> GetByUserAsync(Guid projectId, Guid userId);
    public Task AddAsync(Project task);
    public Task DeleteAsync(Project task);
    public Task SaveChangesAsync();
}
