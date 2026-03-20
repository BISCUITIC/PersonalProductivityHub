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

    public static async Task<IResult> GetAllProjectTasks(Guid projectId,
                                                         ClaimsPrincipal user,
                                                         IProjectRepository projectRepository,
                                                         IProjectTaskRepository taskRepository)
    {
        var (success, result) = await ValidateProjectAccess(projectId, user, projectRepository);
        if (!success) return result!;

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
        var (success, result) = await ValidateProjectAccess(projectId, user, projectRepository);
        if (!success) return result!;

        var (taskSuccess, task, taskError) = await TryGetProjectTask(taskId, projectId, taskRepository);
        if (!taskSuccess) return taskError!;

        ProjectTaskResponse response = task.ToProjectTaskResponse();

        return Results.Ok(response);
    }

    private static async Task<IResult> CreateProjectTask(Guid projectId,
                                                         CreateProjectTaskRequest request,
                                                         ClaimsPrincipal user,
                                                         IProjectRepository projectRepository,
                                                         IProjectTaskRepository taskRepository)
    {
        var (success, result) = await ValidateProjectAccess(projectId, user, projectRepository);
        if (!success) return result!;

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
        var (success, error) = await ValidateProjectAccess(projectId, user, projectRepository);
        if (!success) return error!;

        var (taskSuccess, task, taskError) = await TryGetProjectTask(taskId, projectId, taskRepository);
        if (!taskSuccess) return taskError!;

        if (request.Name is not null)
            task.UpdateName(request.Name);

        if (request.Description is not null)
            task.UpdateDescription(request.Description);

        if (request.Deadline.HasValue)
            task.UpdateDeadline(request.Deadline);

        if (request.Status.HasValue)
            task.UpdateStatus(request.Status.Value);

        await taskRepository.SaveChangesAsync();     

        ProjectTaskResponse response = task.ToProjectTaskResponse();

        return Results.Ok(response);
    }

    private static async Task<IResult> DeleteProjectTask(Guid projectId,
                                                         Guid taskId,
                                                         ClaimsPrincipal user,
                                                         IProjectRepository projectRepository,
                                                         IProjectTaskRepository taskRepository)
    {
        var (success, error) = await ValidateProjectAccess(projectId, user, projectRepository);
        if (!success) return error!;

        var (taskSuccess, task, taskError) = await TryGetProjectTask(taskId, projectId, taskRepository);
        if (!taskSuccess) return taskError!;

        taskRepository.Delete(task);
        await taskRepository.SaveChangesAsync();

        return Results.NoContent();
    }

    private static (bool, Guid?, IResult?) TryGetUserId(ClaimsPrincipal user)
    {
        string? id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        bool success = Guid.TryParse(id, out Guid userId);

        if (!success)
        {
            return (false, null, UserNotAuthenticated());
        }

        return (true, userId, null);
    }

    private static async Task<(bool, Project?, IResult?)> TryGetProject(Guid projectId, Guid userId, IProjectRepository repository)
    {
        Project? project = await repository.GetByIdAsync(projectId, userId);

        if (project is null)
        {
            return (false, null, ProjectNotFound());
        }

        return (true, project, null);
    }

    private static async Task<(bool, ProjectTask?, IResult?)> TryGetProjectTask(Guid taskId, Guid projectId, IProjectTaskRepository repository)
    {
        ProjectTask? task = await repository.GetByIdAsync(taskId, projectId);

        if (task is null)
        {
            return (false, null, ProjectTaskNotFound());
        }

        return (true, task, null);
    }

    private static async Task<(bool, IResult?)> ValidateProjectAccess(Guid projectId,
                                                              ClaimsPrincipal user,
                                                              IProjectRepository repository)
    {
        var (userSuccess, userId, userError) = TryGetUserId(user);
        if (!userSuccess) return (false, userError!);

        var (projectSuccess, project, projectError) = await TryGetProject(projectId, userId.Value, repository);
        if (!projectSuccess) return (false, projectError!);

        return (true, null);
    }

    private static IResult UserNotAuthenticated()
    {
        return Results.Problem(title: "User not authenticated",
                               statusCode: StatusCodes.Status401Unauthorized);
    }

    private static IResult ProjectNotFound()
    {
        return Results.Problem(title: "Project not found",
                               statusCode: StatusCodes.Status404NotFound);
    }

    private static IResult ProjectTaskNotFound()
    {
        return Results.Problem(title: "Project task not found",
                               statusCode: StatusCodes.Status404NotFound);
    }
}
