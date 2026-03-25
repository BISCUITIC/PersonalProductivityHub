using Application.Dtos.Project;
using PersonalProductivityHub.Contracts.Project;

namespace PersonalProductivityHub.Mappings.Requests;

public static class CreateProjectRequestExtensions
{
    public static CreateProjectDto ToCreateProjectDto(this CreateProjectRequest request)
    {
        return new CreateProjectDto(request.Name, request.Description);
    }
}
