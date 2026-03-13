using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationContext _context;

    public ProjectRepository(ApplicationContext context)
    {
        _context = context;
    }

    public Task<List<Project>> GetAllByUserAsync(Guid userId)
    {
        return _context.Projects
                       .Where((project) => project.UserId == userId)
                       .ToListAsync();
    }

    public  Task<Project?> GetByUserAsync(Guid projectId, Guid userId)
    {
        return _context.Projects
                       .FirstOrDefaultAsync((project) => project.UserId == userId && project.Id == projectId);
    }

    public Task AddAsync(Project project)
    {
        _context.Projects.AddAsync(project);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
