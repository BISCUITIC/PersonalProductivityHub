using Domain.Entities;
using PersonalProductivityHub.Contracts.ProjectTask;

namespace PersonalProductivityHub.Mappings;

public static class ProjectTaskExtension
{
    public static ProjectTaskResponse ToProjectTaskResponse(this ProjectTask projectTask)
    {
        return new ProjectTaskResponse(projectTask.Id,
                                       projectTask.Name,
                                       projectTask.Description,
                                       (int)projectTask.Status,
                                       projectTask.Deadline,
                                       projectTask.CreatedAt);
    }
}
