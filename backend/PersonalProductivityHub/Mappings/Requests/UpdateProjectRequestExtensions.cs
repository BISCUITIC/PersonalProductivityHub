using Application.Dtos.Project;
using PersonalProductivityHub.Contracts.Project;

namespace PersonalProductivityHub.Mappings.Requests;

public static class UpdateProjectRequestExtensions
{
    public static UpdateProjectDto ToUpdateProjectDto(this UpdateProjectRequest request)
    {
        return new UpdateProjectDto(request.Name, request.Description);
    }
}
