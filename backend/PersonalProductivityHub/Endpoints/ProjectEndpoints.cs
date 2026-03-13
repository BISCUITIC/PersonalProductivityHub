using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using PersonalProductivityHub.Contracts.Project;
using System.Security.Claims;

namespace PersonalProductivityHub.Endpoints;

public static class ProjectEndpoints
{
    private static string _baseUrl = "/projects";
    public static void MapProjectEndpoints(this WebApplication application)
    {
        RouteGroupBuilder group = application.MapGroup(_baseUrl)
                                             .RequireAuthorization();

        group.MapGet("/", GetAllProjects);
        group.MapGet("/{id:guid}", GetProject);
        group.MapPost("/", CreateProject);
        group.MapPut("/{id:guid}", UpdateProject);
        group.MapDelete("/{id:guid}", DeleteProject);
    }
    private async static Task<Guid?> GetUserIdAsync(ClaimsPrincipal user,
                                               UserManager<ApplicationUser> userManager)
    {
        ApplicationUser? applicationUser = await userManager.GetUserAsync(user);
        return applicationUser?.Id;
    }

    private async static Task<IResult> GetAllProjects(ClaimsPrincipal user,
                                                      UserManager<ApplicationUser> userManager,
                                                      IProjectRepository repository)
    {
        Guid? userId = await GetUserIdAsync(user, userManager);

        if (userId is null)
            return Results.Unauthorized();

        List<Project> projects = await repository.GetAllByUserAsync(userId.Value);

        return Results.Ok(projects);
    }

    private async static Task<IResult> GetProject(Guid id,
                                                  ClaimsPrincipal user,
                                                  UserManager<ApplicationUser> userManager,
                                                  IProjectRepository repository)
    {
        Guid? userId = await GetUserIdAsync(user, userManager);

        if (userId is null)
            return Results.Unauthorized();

        Project? project = await repository.GetByUserAsync(id, userId.Value);

        return Results.Ok(project);
    }

    private async static Task<IResult> CreateProject(ProjectRequest request,
                                                     ClaimsPrincipal user,
                                                     UserManager<ApplicationUser> userManager,
                                                     IProjectRepository repository)
    {
        Guid? userId = await GetUserIdAsync(user, userManager);

        if (userId is null)
            return Results.Unauthorized();

        Project project = new Project(request.Name, request.Description, userId.Value);

        await repository.AddAsync(project);
        await repository.SaveChangesAsync();

        ProjectResponse response = new ProjectResponse(project.Id,
                                                       project.Name,
                                                       project.Description,
                                                       project.CreatedAt);

        return Results.Created($"{_baseUrl}/{project.Id}", response);
    }

    private async static Task<IResult> UpdateProject(Guid id,
                                                     ProjectRequest request,
                                                     ClaimsPrincipal user,
                                                     UserManager<ApplicationUser> userManager,
                                                     IProjectRepository repository)
    {
        Guid? userId = await GetUserIdAsync(user, userManager);

        if (userId is null)
            return Results.Unauthorized();

        Project? project = await repository.GetByUserAsync(id, userId.Value);

        if (project is null)
            return Results.NotFound();

        project.UpdateName(request.Name);
        project.UpdateDescription(request.Description);
        await repository.SaveChangesAsync();

        ProjectResponse response = new ProjectResponse(project.Id,
                                                       project.Name,
                                                       project.Description,
                                                       project.CreatedAt);

        return Results.Ok(response);
    }

    private async static Task<IResult> DeleteProject(Guid id,                                                     
                                                     ClaimsPrincipal user,
                                                     UserManager<ApplicationUser> userManager,
                                                     IProjectRepository repository)
    {
        Guid? userId = await GetUserIdAsync(user, userManager);

        if (userId is null)
            return Results.Unauthorized();

        Project? project = await repository.GetByUserAsync(id, userId.Value);

        if (project is null)
            return Results.NotFound();

        await repository.DeleteAsync(project);
        await repository.SaveChangesAsync();       

        return Results.NoContent();
    }
}
