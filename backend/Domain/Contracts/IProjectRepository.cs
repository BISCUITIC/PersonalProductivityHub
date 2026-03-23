using Domain.Entities;

namespace Domain.Contracts;

public interface IProjectRepository
{
    Task<List<Project>> GetAllByUserAsync(Guid userId);
    Task<Project?> GetByIdAsync(Guid projectId, Guid userId);

    Task<bool> ExistsByNameAsync(string name, Guid userId);
    void Add(Project project);
    void Delete(Project project);

    Task SaveChangesAsync();
}
