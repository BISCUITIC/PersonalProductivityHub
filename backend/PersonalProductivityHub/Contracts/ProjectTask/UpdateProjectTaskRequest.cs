using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalProductivityHub.Contracts.ProjectTask;

public sealed record UpdateProjectTaskRequest(
    [MaxLength(128)]
    string? Name,

    [MaxLength(1024)]
    string? Description,
    
    StatusTask? Status,

    DateTime? Deadline
);
