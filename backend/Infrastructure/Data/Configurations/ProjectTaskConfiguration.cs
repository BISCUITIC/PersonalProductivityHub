using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(task => task.Id);
        builder.HasIndex(task => task.ProjectId);

        builder.Property(task => task.Name)
               .IsRequired()
               .HasMaxLength(128);

        builder.Property(task => task.Description)
               .HasMaxLength(1024);

        builder.Property(task => task.Status)
               .IsRequired();

        builder.Property(task => task.Deadline);

        builder.Property(task => task.CreatedAt)
               .IsRequired();

        builder.HasOne(task => task.Project)
               .WithMany(project => project.Tasks)
               .HasForeignKey(task => task.ProjectId)
               .OnDelete(DeleteBehavior.Cascade); 
    }
}
