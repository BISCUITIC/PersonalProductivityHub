namespace Application.Dtos.ProjectTask;

public sealed record CreateProjectTaskDto
(
    string Name,
    string? Description,
    DateTime? Deadline
);