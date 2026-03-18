using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class ProjectTaskRepository : IProjectTaskRepository
{
    private readonly ApplicationContext _context;

    public ProjectTaskRepository(ApplicationContext context)
    {
        _context = context;
    }
    public Task<List<ProjectTask>> GetAllByProjectAsync(Guid projectId)
    {
        return _context.ProjectTasks
                       .Where(projectTask => projectTask.ProjectId == projectId)
                       .ToListAsync();
    }

    public Task<ProjectTask?> GetByProjectAsync(Guid projectTaskId, Guid projectId)
    {
        return _context.ProjectTasks
                       .FirstOrDefaultAsync(projectTask => projectTask.Id == projectTaskId &&
                                                           projectTask.ProjectId == projectId);
    }

    public Task AddAsync(ProjectTask task)
    {
        _context.ProjectTasks.AddAsync(task);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ProjectTask task)
    {
        _context.ProjectTasks.Remove(task);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
