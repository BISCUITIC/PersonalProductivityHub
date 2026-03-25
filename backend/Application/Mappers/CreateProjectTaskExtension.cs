using Application.Dtos.ProjectTask;
using Domain.Entities;

namespace Application.Mappers;

internal static class CreateProjectTaskExtension
{
    public static ProjectTask ToProjectTask(this CreateProjectTaskDto createDto, Guid projectId)
    {
        return new ProjectTask(createDto.Name,
                               createDto.Description,
                               createDto.Deadline,
                               projectId);
    }
}
