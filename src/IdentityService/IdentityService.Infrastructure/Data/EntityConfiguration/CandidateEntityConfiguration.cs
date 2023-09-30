using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Data.EntityConfiguration;

public class CandidateEntityConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToTable("Candidates");

        builder.HasKey(k => k.Id);
        builder.Property(p => p.Id).HasColumnName("Id");

        builder.Property(p => p.Email)
            .HasColumnName("Email")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(p => p.Password)
            .HasColumnName("Password")
            .HasMaxLength(20)
            .IsRequired();
    }
}
