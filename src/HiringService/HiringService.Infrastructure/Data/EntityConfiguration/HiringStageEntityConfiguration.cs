using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HiringService.Infrastructure.Data.EntityConfiguration;

public class HiringStageEntityConfiguration : IEntityTypeConfiguration<HiringStage>
{
    public void Configure(EntityTypeBuilder<HiringStage> builder)
    {
        builder.ToTable("HiringStages");

        builder.HasKey(k => k.Id);
        builder.Property(p => p.Id).HasColumnName("Id");

        builder.Property(p => p.Description)
            .HasColumnName("Description")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(p => p.DateTime)
           .HasColumnName("DateTime");

        builder.Property(p => p.PassedSuccessfully)
           .HasColumnName("PassedSuccessfully");
    }
}
