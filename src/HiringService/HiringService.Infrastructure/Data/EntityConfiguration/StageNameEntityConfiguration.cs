using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HiringService.Infrastructure.Data.EntityConfiguration;

internal class StageNameEntityConfiguration : IEntityTypeConfiguration<HiringStageName>
{
    public void Configure(EntityTypeBuilder<HiringStageName> builder)
    {
        builder.ToTable("HiringStageNames");

        builder.HasKey(k => k.Id);
        builder.Property(p => p.Id).HasColumnName("Id");

        builder.Property(p => p.Name)
            .HasColumnName("Name")
            .HasMaxLength(60)
            .IsRequired();

        builder.HasMany(m => m.HiringStages)
            .WithOne(o => o.HiringStageName)
            .HasForeignKey(fk => fk.HiringStageNameId)
            .IsRequired()
            .OnDelete(DeleteBehavior.SetNull);
    }
}
