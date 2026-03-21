using Domain.Contracts;
using Domain.Entities;
using PersonalProductivityHub.Contracts.Project;
using PersonalProductivityHub.Mappings;
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
                                                      IProjectRepository repository)
    {
        var (userSuccess, userId, userError) = TryGetUserId(user);
        if (!userSuccess) return userError!;

        List<Project> projects = await repository.GetAllByUserAsync(userId.Value);

        List<ProjectResponse> response = projects.Select(project => project.ToProjectResponse()).ToList();

        return Results.Ok(response);
    }

    private static async Task<IResult> GetProject(Guid id,
                                                  ClaimsPrincipal user,
                                                  IProjectRepository repository)
    {
        var (userSuccess, userId, userError) = TryGetUserId(user);
        if (!userSuccess) return userError!;

        var (projectSuccess, project, projectError) = await TryGetProject(id, userId.Value, repository);
        if (!projectSuccess) return projectError!;

        ProjectResponse response = project!.ToProjectResponse();

        return Results.Ok(response);
    }

    private static async Task<IResult> CreateProject(ProjectRequest request,
                                                     ClaimsPrincipal user,
                                                     IProjectRepository repository)
    {
        var (success, userId, error) = TryGetUserId(user);
        if (!success) return error!;

        Project project = new Project(request.Name, request.Description, userId.Value);

        repository.Add(project);
        await repository.SaveChangesAsync();

        ProjectResponse response = project.ToProjectResponse();

        return Results.Created($"/projects/{response.Id}", response);
    }

    private static async Task<IResult> UpdateProject(Guid id,
                                                     ProjectRequest request,
                                                     ClaimsPrincipal user,
                                                     IProjectRepository repository)
    {
        var (userSuccess, userId, userError) = TryGetUserId(user);
        if (!userSuccess) return userError!;

        var (projectSuccess, project, projectError) = await TryGetProject(id, userId.Value, repository);
        if (!projectSuccess) return projectError!;

        project!.UpdateName(request.Name);
        project!.UpdateDescription(request.Description);
        await repository.SaveChangesAsync();

        ProjectResponse response = project!.ToProjectResponse();

        return Results.Ok(response);
    }

    private static async Task<IResult> DeleteProject(Guid id,
                                                     ClaimsPrincipal user,
                                                     IProjectRepository repository)
    {
        var (userSuccess, userId, userError) = TryGetUserId(user);
        if (!userSuccess) return userError!;

        var (projectSuccess, project, projectError) = await TryGetProject(id, userId.Value, repository);
        if (!projectSuccess) return projectError!;

        repository.Delete(project!);
        await repository.SaveChangesAsync();

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
}
