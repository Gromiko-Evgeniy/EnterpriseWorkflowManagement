using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiringService.Infrastructure.Data.EntityConfiguration;

public class CandidateEntityConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToTable("Candidates");

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

        builder.Property(p => p.CV)
            .HasColumnName("CV")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.NextStageTime)
           .HasColumnName("NextStageTime");

        builder.HasMany(m => m.HiringStages)
            .WithOne(o => o.Candidate)
            .HasForeignKey(fk => fk.CandidateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.SetNull);
    }
}
