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

    public ProjectTask(string name, string? description, DateTime? deadline, Guid projectId)
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidateDeadline(deadline);

        Id = Guid.NewGuid();

        Name = name;
        Description = description;
        Status = StatusTask.New; 
        Deadline = deadline;

        ProjectId = projectId;

        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateName(string name)
    {
        ValidateName(name);
        Name = name;
    }

    public void UpdateDescription(string? description)
    {
        ValidateDescription(description);
        Description = description;
    }

    public void UpdateStatus(StatusTask status)
    {
        Status = status;
    }

    public void UpdateDeadline(DateTime? deadline)
    {
        ValidateDeadline(deadline);
        Deadline = deadline;
    }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Task name is required", nameof(name));

        if (name.Length > 128)
            throw new ArgumentException("Task name is too long", nameof(name));
    }

    private void ValidateDescription(string? description)
    {
        if (description?.Length > 1024)
            throw new ArgumentException("Description is too long", nameof(description));
    }

    private void ValidateDeadline(DateTime? deadline)
    {
        if (deadline.HasValue && deadline.Value <= DateTime.UtcNow)
            throw new ArgumentException("Deadline cannot be in the past", nameof(deadline));
    }
}
