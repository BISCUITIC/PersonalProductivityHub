using Domain.Contracts;
using Domain.Entities;
using PersonalProductivityHub.Contracts.ProjectTask;
using PersonalProductivityHub.Mappings;
using System.Security.Claims;

namespace PersonalProductivityHub.Endpoints;

public static class ProjectTaskEndpoints
{
    private const string BASE_URL = "/projects/{projectId:guid}/tasks";

    public static void MapProjectTaskEndpoints(this WebApplication application)
    {
        RouteGroupBuilder group = application.MapGroup(BASE_URL)
                                             .RequireAuthorization();

        group.MapGet("/", GetAllProjectTasks);
        group.MapGet("/{taskId:guid}", GetProjectTask);
        group.MapPost("/", CreateProjectTask);
        group.MapPatch("/{taskId:guid}", UpdateProjectTask);
        group.MapDelete("/{taskId:guid}", DeleteProjectTask);
    }

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        string? id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        bool result = Guid.TryParse(id, out Guid userId);
        return result == true ? userId : null;
    }

    private static async Task<IResult?> ValidateProjectAccess(Guid projectId,
                                                              ClaimsPrincipal user,
                                                              IProjectRepository repository)
    {
        Guid? userId = GetUserId(user);
        if (userId is null)
            return Results.Unauthorized();

        Project? project = await repository.GetByIdAsync(projectId, userId.Value);
        if (project is null)
            return Results.NotFound();

        return null;
    }

    public static async Task<IResult> GetAllProjectTasks(Guid projectId,
                                                         ClaimsPrincipal user,
                                                         IProjectRepository projectRepository,
                                                         IProjectTaskRepository taskRepository)
    {
        IResult? result = await ValidateProjectAccess(projectId, user, projectRepository);

        if (result is not null)
            return result;

        List<ProjectTask> task = await taskRepository.GetAllByProjectAsync(projectId);

        List<ProjectTaskResponse> response = task.Select(task => task.ToProjectTaskResponse())
                                                 .ToList();

        return Results.Ok(response);
    }

    private static async Task<IResult> GetProjectTask(Guid projectId,
                                                      Guid taskId,
                                                      ClaimsPrincipal user,
                                                      IProjectRepository projectRepository,
                                                      IProjectTaskRepository taskRepository)
    {
        IResult? result = await ValidateProjectAccess(projectId, user, projectRepository);

        if (result is not null)
            return result;

        ProjectTask? task = await taskRepository.GetByIdAsync(taskId, projectId);
        if (task is null)
            return Results.NotFound();

        ProjectTaskResponse response = task.ToProjectTaskResponse();

        return Results.Ok(response);
    }

    private static async Task<IResult> CreateProjectTask(Guid projectId,
                                                         CreateProjectTaskRequest request,
                                                         ClaimsPrincipal user,
                                                         IProjectRepository projectRepository,
                                                         IProjectTaskRepository taskRepository)
    {
        IResult? result = await ValidateProjectAccess(projectId, user, projectRepository);

        if (result is not null)
            return result;

        ProjectTask task = new ProjectTask(request.Name, request.Description, request.Deadline, projectId);
        taskRepository.Add(task);
        await taskRepository.SaveChangesAsync();

        ProjectTaskResponse response = task.ToProjectTaskResponse();

        return Results.Created($"/projects/{projectId}/tasks/{response.Id}", response);
    }

    private static async Task<IResult> UpdateProjectTask(Guid projectId,
                                                         Guid taskId,
                                                         UpdateProjectTaskRequest request,
                                                         ClaimsPrincipal user,
                                                         IProjectRepository projectRepository,
                                                         IProjectTaskRepository taskRepository)
    {
        IResult? result = await ValidateProjectAccess(projectId, user, projectRepository);

        if (result is not null)
            return result;

        ProjectTask? task = await taskRepository.GetByIdAsync(taskId, projectId);
        if (task is null)
            return Results.NotFound();
        try
        {
            if (request.Name is not null)
                task.UpdateName(request.Name);

            if (request.Description is not null)
                task.UpdateDescription(request.Description);

            if (request.Deadline.HasValue)
                task.UpdateDeadline(request.Deadline);

            if (request.Status.HasValue)
                task.UpdateStatus(request.Status.Value);

            await taskRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }

        ProjectTaskResponse response = task.ToProjectTaskResponse();

        return Results.Ok(response);
    }

    private static async Task<IResult> DeleteProjectTask(Guid projectId,
                                                         Guid taskId,
                                                         ClaimsPrincipal user,
                                                         IProjectRepository projectRepository,
                                                         IProjectTaskRepository taskRepository)
    {
        IResult? result = await ValidateProjectAccess(projectId, user, projectRepository);

        if (result is not null)
            return result;

        ProjectTask? task = await taskRepository.GetByIdAsync(taskId, projectId);
        if (task is null)
            return Results.NotFound();

        taskRepository.Delete(task);
        await taskRepository.SaveChangesAsync();

        return Results.NoContent();
    }
}
