using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

        builder.HasMany(m => m.HiringStages)
            .WithOne(o => o.Intervier)
            .HasForeignKey(fk => fk.IntervierId)
            .IsRequired()
            .OnDelete(DeleteBehavior.SetNull);
    }
}