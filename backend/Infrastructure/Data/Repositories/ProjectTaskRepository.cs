using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

//TODO add AsNoTracking 

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

    public Task<ProjectTask?> GetByIdAsync(Guid projectTaskId, Guid projectId)
    {
        return _context.ProjectTasks
                       .FirstOrDefaultAsync(projectTask => projectTask.Id == projectTaskId &&
                                                           projectTask.ProjectId == projectId);
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
