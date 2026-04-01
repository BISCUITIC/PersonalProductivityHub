using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalProductivityHub.Contracts.ProjectTask;

public sealed record UpdateProjectTaskRequest(
    [Required, MaxLength(128)]
    string? Name,

    [MaxLength(1024)]
    string? Description,
    
    StatusTask? Status,

    string? Deadline
);
