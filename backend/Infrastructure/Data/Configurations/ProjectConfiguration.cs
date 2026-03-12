using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(project => project.Id);
        builder.HasIndex(project => project.UserId);

        builder.Property(project => project.Name)
               .IsRequired()
               .HasMaxLength(128);

        builder.Property(project => project.Description)
               .HasMaxLength(1024);

        builder.Property(project => project.CreatedAt)
               .IsRequired();
        
        builder.HasOne<ApplicationUser>()
               .WithMany() 
               .HasForeignKey(project => project.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
