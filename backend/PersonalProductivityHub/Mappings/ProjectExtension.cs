using Domain.Entities;
using PersonalProductivityHub.Contracts.Project;

namespace PersonalProductivityHub.Mappings;

public static class ProjectExtension
{
    public static ProjectResponse ToProjectResponse(this Project project)
    {
        return new ProjectResponse(project.Id, 
                                   project.Name, 
                                   project.Description, 
                                   project.CreatedAt);
    }
}
