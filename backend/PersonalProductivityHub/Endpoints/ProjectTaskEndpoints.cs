using Application.Common.Result;
using Application.Contracts;
using Application.Dtos.ProjectTask;
using PersonalProductivityHub.Contracts.ProjectTask;
using PersonalProductivityHub.Mappings;
using PersonalProductivityHub.Mappings.Requests;

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
                                                         IProjectTaskService service)
    {
        Result<List<ProjectTaskDto>> result = await service.GetAllTasks(projectId);

        return result.ToHttpResult(
            (tasks) => tasks.Select(task => task.ToProjectTaskResponse())
        );
    }

    private static async Task<IResult> GetProjectTask(Guid projectId,
                                                      Guid taskId,
                                                      IProjectTaskService service)
    {
        Result<ProjectTaskDto> result = await service.GetTaskById(taskId, projectId);

        return result.ToHttpResult(
            (project) => project.ToProjectTaskResponse()
        );
    }

    private static async Task<IResult> CreateProjectTask(Guid projectId,
                                                         CreateProjectTaskRequest request,
                                                         IProjectTaskService service)
    {
        Result<ProjectTaskDto> result = await service.AddTask(projectId,
                                                              request.ToCreateProjectTaskDto());

        return result.ToHttpResult(
            (task) => task.ToProjectTaskResponse(),
            (response) => $"/projects/{projectId}/tasks/{response.Id}"
        );
    }

    private static async Task<IResult> UpdateProjectTask(Guid projectId,
                                                         Guid taskId,
                                                         UpdateProjectTaskRequest request,
                                                         IProjectTaskService service)
    {
        Result<ProjectTaskDto> result = await service.UpdateTask(taskId,
                                                                 projectId,
                                                                 request.ToUpdateProjectTaskDto());

        return result.ToHttpResult(
            (task) => task.ToProjectTaskResponse()
        );
    }

    private static async Task<IResult> DeleteProjectTask(Guid projectId,
                                                         Guid taskId,
                                                         IProjectTaskService service)
    {
        Result result = await service.DeleteTask(taskId, projectId);

        return result.ToHttpResult();
    }
}