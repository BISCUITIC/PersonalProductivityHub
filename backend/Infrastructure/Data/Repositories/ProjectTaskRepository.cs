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
    public Task<List<ProjectTask>> GetAllByProjectAsync(Guid projectId, Guid userId)
    {
        return _context.ProjectTasks
                       .AsNoTracking()
                       .Where(task => task.Project.UserId == userId)
                       .Where(task => task.ProjectId  == projectId)                       
                       .ToListAsync();
    }

    public Task<ProjectTask?> GetByIdAsync(Guid taskId, Guid projectId, Guid userId)
    {
        return _context.ProjectTasks
                       .Where(task => task.Project.UserId == userId)
                       .Where(task => task.ProjectId == projectId)
                       .Where(task => task.Id == taskId)
                       .FirstOrDefaultAsync();
    }

    public void Add(ProjectTask task)
    {
        _context.ProjectTasks.Add(task);        
    }

    public void Delete(ProjectTask task)
    {
        _context.ProjectTasks.Remove(task);       
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
