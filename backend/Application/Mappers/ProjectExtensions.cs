using Application.Dtos;
using Domain.Entities;

namespace Application.Mappers;

internal static class ProjectExtensions
{
    public static ProjectDto ToProjectDto(this Project project)
    {
        return new ProjectDto(project.Id, project.Name, project.Description, project.CreatedAt);
    }
}
