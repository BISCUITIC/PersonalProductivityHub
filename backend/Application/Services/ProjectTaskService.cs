using Application.Common.Result;
using Application.Contracts;
using Application.Dtos.ProjectTask;
using Application.Mappers;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services;

public class ProjectTaskService : IProjectTaskService
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUser _user;

    public ProjectTaskService(IProjectTaskRepository taskRepository, 
                              IProjectRepository projectRepository,
                              ICurrentUser user)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _user = user;
    }

    public async Task<Result<List<ProjectTaskDto>>> GetAllTasks(Guid projectId)
    {
        List<ProjectTask> tasks = await _taskRepository.GetAllByProjectAsync(projectId, _user.UserId);
        List<ProjectTaskDto> results = tasks.Select(task => task.ToProjectTaskDto()).ToList();

        return Result<List<ProjectTaskDto>>.Success(results, ResultStatus.Success);
    }

    public async Task<Result<ProjectTaskDto>> GetTaskById(Guid taskId, Guid projectId)
    {
        ProjectTask? task = await _taskRepository.GetByIdAsync(taskId, projectId, _user.UserId);

        if(task is null) 
            return NotFound<ProjectTaskDto>();
        
        return Result<ProjectTaskDto>.Success(task.ToProjectTaskDto(), ResultStatus.Success);
    }

    public async Task<Result<ProjectTaskDto>> AddTask(Guid projectId, CreateProjectTaskDto createDto)
    {
        Project? project = await _projectRepository.GetByIdAsync(projectId, _user.UserId);
        if (project is null)
            return ProjectNotFound<ProjectTaskDto>();

        ProjectTask task = createDto.ToProjectTask(projectId);

        _taskRepository.Add(task);
        await _taskRepository.SaveChangesAsync();

        return Result<ProjectTaskDto>.Success(task.ToProjectTaskDto(), ResultStatus.Created);
    }

    public async Task<Result> DeleteTask(Guid taskId, Guid projectId)
    {
        ProjectTask? task = await _taskRepository.GetByIdAsync(taskId, projectId, _user.UserId);

        if (task is null)
            return NotFound();

        _taskRepository.Delete(task);
        await _taskRepository.SaveChangesAsync();

        return Result.Success(ResultStatus.NoContent);
    }


    public async Task<Result<ProjectTaskDto>> UpdateTask(Guid taskId, Guid projectId, UpdateProjectTaskDto updateDto)
    {
        ProjectTask? task = await _taskRepository.GetByIdAsync(taskId, projectId, _user.UserId);

        if (task is null)
            return NotFound<ProjectTaskDto>();

        if (updateDto.Name is not null)
            task.UpdateName(updateDto.Name);

        if(updateDto.Description is not null)
            task.UpdateDescription(updateDto.Description);

        if (updateDto.Status is not null)
            task.UpdateStatus(updateDto.Status.Value);

        if (updateDto.Deadline is not null)
            task.UpdateDeadline(updateDto.Deadline);

        await _taskRepository.SaveChangesAsync();

        return Result<ProjectTaskDto>.Success(task.ToProjectTaskDto(), ResultStatus.Success);
    }

    private static Result<T> NotFound<T>() =>
        Result<T>.Failure("Task not found", ResultStatus.NotFound);

    private static Result NotFound() =>
        Result.Failure("Task not found", ResultStatus.NotFound);

    private static Result<T> ProjectNotFound<T>() =>
        Result<T>.Failure("Project not found", ResultStatus.NotFound);
}
