using Domain.Enums;

namespace Application.Dtos.ProjectTask;

public sealed record class ProjectTaskDto
(
    Guid Id,
    string Name,
    string? Description,
    StatusTask Status,
    DateTime? Deadline,
    DateTime CreatedAt
);

