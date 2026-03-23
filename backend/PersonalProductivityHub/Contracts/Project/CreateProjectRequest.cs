using System.ComponentModel.DataAnnotations;

namespace PersonalProductivityHub.Contracts.Project;

public sealed record class CreateProjectRequest
(
    [Required, MaxLength(128)]
    string Name,

    [MaxLength(1024)]
    string? Description
);
