using Domain.Entities;
using Infrastructure.Data.Configurations;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectTask> ProjectTasks { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
         : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectTaskConfiguration());
    }
}
