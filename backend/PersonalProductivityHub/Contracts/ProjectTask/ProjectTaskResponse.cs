using System.ComponentModel.DataAnnotations;

namespace PersonalProductivityHub.Contracts.ProjectTask;

public sealed record ProjectTaskResponse
(
    [Required]
    Guid Id,

    [Required, MaxLength(128)]
    string Name,

    [MaxLength(1024)]
    string? Description,

    [Required]
    int Status,
    
    DateTime? Deadline,

    [Required]
    DateTime CreatedAt
);