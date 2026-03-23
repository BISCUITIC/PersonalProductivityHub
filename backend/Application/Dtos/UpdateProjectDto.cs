namespace Application.Dtos;

public sealed record class UpdateProjectDto
(
    string? Name,
    string? Description
);