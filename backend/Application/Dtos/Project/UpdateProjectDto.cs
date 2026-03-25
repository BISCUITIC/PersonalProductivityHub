namespace Application.Dtos.Project;

public sealed record class UpdateProjectDto
(
    string? Name,
    string? Description
);