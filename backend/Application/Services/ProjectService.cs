using Application.Common.Result;
using Application.Contracts;
using Application.Dtos;
using Application.Mappers;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ProjectDto>>> GetAllProjects(Guid userId)
    {
        List<Project> projects = await _repository.GetAllByUserAsync(userId);
        List<ProjectDto> result = projects.Select(project => project.ToProjectDto()).ToList();

        return Result<List<ProjectDto>>.Success(result, ResultStatus.Success);
    }

    public async Task<Result<ProjectDto>> GetProjectById(Guid projectId, Guid userId)
    {
        Project? project = await _repository.GetByIdAsync(projectId, userId);

        if (project is null)
            return NotFound<ProjectDto>();

        return Result<ProjectDto>.Success(project.ToProjectDto(), ResultStatus.Success);  
    }

    public async Task<Result<ProjectDto>> AddProject(Guid userId, CreateProjectDto createDto)
    {
        Project project = createDto.ToProject(userId);

        if(await _repository.ExistsByNameAsync(project.Name, project.UserId))
            return AlreadyExists<ProjectDto>();

        _repository.Add(project);
        await _repository.SaveChangesAsync();

        return Result<ProjectDto>.Success(project.ToProjectDto(), ResultStatus.Created);
    }

    public async Task<Result> DeleteProject(Guid projectId, Guid userId)
    {
        Project? project = await _repository.GetByIdAsync(projectId, userId);

        if (project is null)
            return NotFound();

        _repository.Delete(project);
        await _repository.SaveChangesAsync();

        return Result.Success(ResultStatus.NoContent);
    }

    public async Task<Result<ProjectDto>> UpdateProject(Guid projectId, Guid userId, UpdateProjectDto updateDto)
    {
        Project? project = await _repository.GetByIdAsync(projectId, userId);

        if (project is null)
            return NotFound<ProjectDto>();

        if(updateDto.Name is not null)
        {
            bool isSameName = updateDto.Name == project.Name;
            bool alreadyExists = await _repository.ExistsByNameAsync(updateDto.Name, userId);

            if (alreadyExists && !isSameName)           
                return AlreadyExists<ProjectDto>();
            
            project.UpdateName(updateDto.Name);
        }

        if (updateDto.Description is not null)
        {
            project.UpdateDescription(updateDto.Description);
        }

        await _repository.SaveChangesAsync();

        return Result<ProjectDto>.Success(project.ToProjectDto(), ResultStatus.Success);
    }

    private static Result<T> NotFound<T>() => 
        Result<T>.Failure("Project not found", ResultStatus.NotFound);
    private static Result NotFound() => 
        Result.Failure("Project not found", ResultStatus.NotFound);
    private static Result<T> AlreadyExists<T>() => 
        Result<T>.Failure("Project with this name already exists", ResultStatus.NotFound);
}
