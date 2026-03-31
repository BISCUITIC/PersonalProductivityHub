using Application.Dtos.ProjectTask;
using PersonalProductivityHub.Contracts.ProjectTask;

namespace PersonalProductivityHub.Mappings.Requests;

public static class UpdateProjectTaskRequestExtensions
{
    public static UpdateProjectTaskDto ToUpdateProjectTaskDto(this UpdateProjectTaskRequest request)
    {
        return new UpdateProjectTaskDto(request.Name,
                                        request.Description,
                                        request.Status,
                                        request.Deadline);
    }
}
