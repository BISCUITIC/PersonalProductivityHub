using Application.Common.Result;
using Application.Dtos.ProjectTask;

namespace Application.Contracts;

public interface IProjectTaskService
{
    Task<Result<List<ProjectTaskDto>>> GetAllTasks(Guid projectId);
    Task<Result<ProjectTaskDto>> GetTaskById(Guid taskId, Guid projectId);

    Task<Result<ProjectTaskDto>> AddTask(Guid projectId, CreateProjectTaskDto dto);
    Task<Result<ProjectTaskDto>> UpdateTask(Guid taskId, Guid projectId, UpdateProjectTaskDto dto);
    Task<Result> DeleteTask(Guid taskId, Guid projectId);
}
