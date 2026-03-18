using System.ComponentModel.DataAnnotations;

namespace PersonalProductivityHub.Contracts.ProjectTask;

public sealed record CreateProjectTaskRequest(
    [Required, MaxLength(128)]
    string Name,

    [MaxLength(1024)]
    string? Description,

    DateTime? Deadline
);

