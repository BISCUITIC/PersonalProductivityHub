using Application.Dtos;
using PersonalProductivityHub.Contracts.Project;

namespace PersonalProductivityHub.Mappings;

public static class ProjectDtoExtensions
{
    public static ProjectResponse ToProjectResponse(this ProjectDto projectDto)
    {
        return new ProjectResponse(projectDto.Id, 
                                   projectDto.Name, 
                                   projectDto.Description, 
                                   projectDto.CreatedAt);
    }
}
