using Application.Dtos;
using PersonalProductivityHub.Contracts.Project;

namespace PersonalProductivityHub.Mappings;

public static class ProjectRequestExtensions
{
    public static CreateProjectDto ToCreateProjectDto(this ProjectRequest request)
    {
        return new CreateProjectDto(request.Name, request.Description);
    }

    public static UpdateProjectDto ToupdateProjectDto(this ProjectRequest request)
    {
        return new UpdateProjectDto(request.Name, request.Description);
    }
}
