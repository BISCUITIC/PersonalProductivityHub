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

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        string? id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        bool result = Guid.TryParse(id, out Guid userId);
        return result == true ? userId : null;
    }

    private static async Task<IResult> GetAllProjects(ClaimsPrincipal user,
                                                      IProjectRepository repository)
    {
        Guid? userId = GetUserId(user);

        if (userId is null)
            return Results.Unauthorized();

        List<Project> projects = await repository.GetAllByUserAsync(userId.Value);

        List<ProjectResponse> response = projects.Select(project => project.ToProjectResponse()).ToList();

        return Results.Ok(response);
    }

    private static async Task<IResult> GetProject(Guid id,
                                                  ClaimsPrincipal user,
                                                  IProjectRepository repository)
    {
        Guid? userId = GetUserId(user);

        if (userId is null)
            return Results.Unauthorized();

        Project? project = await repository.GetByIdAsync(id, userId.Value);

        if (project is null)
            return Results.NotFound();

        ProjectResponse response = project.ToProjectResponse();

        return Results.Ok(response);
    }

    private static async Task<IResult> CreateProject(ProjectRequest request,
                                                     ClaimsPrincipal user,
                                                     IProjectRepository repository)
    {
        Guid? userId = GetUserId(user);

        if (userId is null)
            return Results.Unauthorized();

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
        Guid? userId = GetUserId(user);

        if (userId is null)
            return Results.Unauthorized();

        Project? project = await repository.GetByIdAsync(id, userId.Value);

        if (project is null)
            return Results.NotFound();

        project.UpdateName(request.Name);
        project.UpdateDescription(request.Description);
        await repository.SaveChangesAsync();

        ProjectResponse response = project.ToProjectResponse();

        return Results.Ok(response);
    }

    private static async Task<IResult> DeleteProject(Guid id,
                                                     ClaimsPrincipal user,
                                                     IProjectRepository repository)
    {
        Guid? userId = GetUserId(user);

        if (userId is null)
            return Results.Unauthorized();

        Project? project = await repository.GetByIdAsync(id, userId.Value);

        if (project is null)
            return Results.NotFound();

        repository.Delete(project);
        await repository.SaveChangesAsync();

        return Results.NoContent();
    }
}
