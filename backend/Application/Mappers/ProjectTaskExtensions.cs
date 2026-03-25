using Application.Dtos.Project;
using Application.Dtos.ProjectTask;
using Domain.Entities;

namespace Application.Mappers;

internal static class ProjectTaskExtensions
{
    public static ProjectTaskDto ToProjectTaskDto(this ProjectTask project)
    {
        return new ProjectTaskDto(project.Id, 
                                  project.Name, 
                                  project.Description, 
                                  project.Status,
                                  project.Deadline,
                                  project.CreatedAt);
    }
}
