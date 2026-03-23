using Application.Common.Result;
using Application.Dtos;

namespace Application.Contracts;

public interface IProjectService
{
    Task<Result<List<ProjectDto>>> GetAllProjects(Guid userId);
    Task<Result<ProjectDto>> GetProjectById(Guid projectId, Guid userId);

    Task<Result<ProjectDto>> AddProject(Guid userId, CreateProjectDto createDto);
    Task<Result<ProjectDto>> UpdateProject(Guid projectId, Guid userId, UpdateProjectDto updateDto);
    Task<Result> DeleteProject(Guid projectId, Guid userId);
}
