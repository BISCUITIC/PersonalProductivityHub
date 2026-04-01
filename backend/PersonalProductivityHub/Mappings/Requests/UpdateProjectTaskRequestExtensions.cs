using Application.Dtos.ProjectTask;
using PersonalProductivityHub.Contracts.ProjectTask;

namespace PersonalProductivityHub.Mappings.Requests;

public static class UpdateProjectTaskRequestExtensions
{
    public static UpdateProjectTaskDto ToUpdateProjectTaskDto(this UpdateProjectTaskRequest request)
    {
        DateTime? deadline = null; 
        bool success = DateTime.TryParse(request.Deadline, out DateTime parsedDeadline);

        if (success)
            deadline = parsedDeadline;

        return new UpdateProjectTaskDto(request.Name,
                                        request.Description,
                                        request.Status,
                                        deadline);
    }
}
