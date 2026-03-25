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
                                       projectDto.Status.ToString(),
                                       projectDto.Deadline,
                                       projectDto.CreatedAt);
    }
}
