using Application.Common.Result;
using Application.Contracts;
using Application.Dtos.Project;
using PersonalProductivityHub.Contracts.Project;
using PersonalProductivityHub.Mappings;
using PersonalProductivityHub.Mappings.Requests;
using System.Security.Claims;

namespace PersonalProductivityHub.Endpoints;

public static class ProjectEndpoints
{
    private const string BASE_URL = "/projects";

    public static void MapProjectEndpoints(this WebApplication application)
    {
        RouteGroupBuilder group = application.MapGroup(BASE_URL)
                                             .RequireAuthorization();

        group.MapGet("/", GetAllProjects);
        group.MapGet("/{id:guid}", GetProject);
        group.MapPost("/", CreateProject);
        group.MapPut("/{id:guid}", UpdateProject);
        group.MapDelete("/{id:guid}", DeleteProject);
    }

    private static async Task<IResult> GetAllProjects(ClaimsPrincipal user,
                                                      IProjectService service)
    {
        var (userSuccess, userId, userError) = TryGetUserId(user);
        if (!userSuccess) return userError!;

        Result<List<ProjectDto>> result = await service.GetAllProjects(userId.Value);

        return result.ToHttpResult(
            (projects) => projects.Select(project => project.ToProjectResponse())
        );
    }

    private static async Task<IResult> GetProject(Guid id,
                                                  ClaimsPrincipal user,
                                                  IProjectService service)
    {
        var (userSuccess, userId, userError) = TryGetUserId(user);
        if (!userSuccess) return userError!;

        Result<ProjectDto> result = await service.GetProjectById(id, userId.Value);

        return result.ToHttpResult(
            (project) => project.ToProjectResponse()
        );
    }

    private static async Task<IResult> CreateProject(CreateProjectRequest request,
                                                     ClaimsPrincipal user,
                                                     IProjectService service)
    {
        var (success, userId, error) = TryGetUserId(user);
        if (!success) return error!;


        Result<ProjectDto> result = await service.AddProject(userId.Value, request.ToCreateProjectDto());

        return result.ToHttpResult(
            (project) => project.ToProjectResponse(),
            (response) => $"/projects/{response.Id}"
        );
    }

    private static async Task<IResult> UpdateProject(Guid id,
                                                     UpdateProjectRequest request,
                                                     ClaimsPrincipal user,
                                                     IProjectService service)
    {
        var (userSuccess, userId, userError) = TryGetUserId(user);
        if (!userSuccess) return userError!;

        Result<ProjectDto> result = await service.UpdateProject(id, userId.Value, request.ToUpdateProjectDto());

        return result.ToHttpResult();
    }

    private static async Task<IResult> DeleteProject(Guid id,
                                                     ClaimsPrincipal user,
                                                     IProjectService service)
    {
        var (userSuccess, userId, userError) = TryGetUserId(user);
        if (!userSuccess) return userError!;

        Result result = await service.DeleteProject(id, userId.Value);

        return result.ToHttpResult();
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

    private static IResult UserNotAuthenticated()
    {
        return Results.Problem(title: "User not authenticated",
                               statusCode: StatusCodes.Status401Unauthorized);
    }
}
