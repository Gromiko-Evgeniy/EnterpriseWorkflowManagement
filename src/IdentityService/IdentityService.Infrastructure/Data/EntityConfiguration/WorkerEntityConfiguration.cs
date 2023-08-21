using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IdentityService.Domain.Entities;

namespace HiringService.Infrastructure.Data.EntityConfiguration;

internal class WorkerEntityConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.ToTable("Workers");

        builder.HasKey(k => k.Id);
        builder.Property(p => p.Id).HasColumnName("Id");

        builder.Property(p => p.Name)
            .HasColumnName("Name")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(p => p.Email)
            .HasColumnName("Email")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(p => p.Password)
            .HasColumnName("Password")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Position)
            .HasColumnName("Position");
    }
}