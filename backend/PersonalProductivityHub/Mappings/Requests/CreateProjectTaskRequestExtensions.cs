using Application.Dtos.ProjectTask;
using PersonalProductivityHub.Contracts.ProjectTask;

namespace PersonalProductivityHub.Mappings.Requests;

public static class CreateProjectTaskRequestExtensions
{
    public static CreateProjectTaskDto ToCreateProjectTaskDto(this CreateProjectTaskRequest request)
    {
        return new CreateProjectTaskDto(request.Name,
                                        request.Description,
                                        request.Deadline);
    }
}
