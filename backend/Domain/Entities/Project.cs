namespace Domain.Entities;

public class Project
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public Guid UserId { get; private set; }
    

    private readonly List<ProjectTask> _tasks = new List<ProjectTask>();
    public IReadOnlyCollection<ProjectTask> Tasks => _tasks;

    public DateTime CreatedAt { get; private set; }

    private Project() { }

    public Project(string name, string? description, Guid userId)
    {
        ValidateName(name);
        ValidateDescription(description);

        Id = Guid.NewGuid();

        Name = name;
        Description = description;

        UserId = userId;        

        CreatedAt = DateTime.UtcNow;
    }

    public void AddTask(ProjectTask task)
    {
        ValidateTask(task);
        _tasks.Add(task);
    }

    public void RemoveTask(ProjectTask task)
    {
        ValidateTask(task);
        _tasks.Remove(task);
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

    private void ValidateTask(ProjectTask task)
    {
        if (task.ProjectId != this.Id)
            throw new InvalidOperationException("Task does not belong to this project.");
    }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Task name is required", nameof(name));

        if (name.Length > 128)
            throw new ArgumentException("Project name is too long", nameof(name));
    }

    private void ValidateDescription(string? description)
    {
        if (description?.Length > 1024)
            throw new ArgumentException("Description is too long", nameof(description));
    }
}
