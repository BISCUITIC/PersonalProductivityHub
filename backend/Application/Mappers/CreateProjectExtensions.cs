using Application.Dtos.Project;
using Domain.Entities;

namespace Application.Mappers;

internal static class CreateProjectExtensions
{
    public static Project ToProject(this CreateProjectDto createDto, Guid userId)
    {
        return new Project(createDto.Name, createDto.Description, userId);
    }
}
