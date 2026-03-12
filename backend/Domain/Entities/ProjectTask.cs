using Domain.Enums;

namespace Domain.Entities;

public class ProjectTask
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public StatusTask Status { get; private set; }
    public DateTime? Deadline { get; private set; }

    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    private ProjectTask() { }

    public ProjectTask(string name, string? description, DateTime? dateTime, Guid projectId)
    {
        Validate(name);

        Id = Guid.NewGuid();

        Name = name;
        Description = description;
        Status = StatusTask.New; 
        Deadline = dateTime;

        ProjectId = projectId;

        CreatedAt = DateTime.UtcNow;
    }

    private void Validate(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Task name is required", nameof(name));
    }
}
