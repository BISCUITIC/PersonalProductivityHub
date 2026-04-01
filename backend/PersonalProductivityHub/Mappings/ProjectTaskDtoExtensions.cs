using Application.Dtos.ProjectTask;
using PersonalProductivityHub.Contracts.ProjectTask;

namespace PersonalProductivityHub.Mappings;


public static class ProjectTaskDtoExtensions
{
    public static ProjectTaskResponse ToProjectTaskResponse(this ProjectTaskDto projectDto)
    {
        return new ProjectTaskResponse(projectDto.Id,
                                       projectDto.Name,
                                       projectDto.Description,
                                       (int)projectDto.Status,
                                       projectDto.Deadline,
                                       projectDto.CreatedAt);
    }
}
