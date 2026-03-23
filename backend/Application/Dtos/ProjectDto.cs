namespace Application.Dtos;

public sealed record class ProjectDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt
);
