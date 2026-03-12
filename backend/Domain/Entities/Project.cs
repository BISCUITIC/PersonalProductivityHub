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
        ValidateModel(name);

        Id = Guid.NewGuid();

        Name = name;
        Description = description;

        UserId = userId;        

        CreatedAt = DateTime.UtcNow;
    }

    private void ValidateModel(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name is required", nameof(name));
    }
}
