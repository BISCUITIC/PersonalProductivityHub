using Domain.Enums;

namespace Application.Dtos.ProjectTask;

public sealed record UpdateProjectTaskDto
(
    string? Name,
    string? Description,
    StatusTask? Status,
    DateTime? Deadline
);
