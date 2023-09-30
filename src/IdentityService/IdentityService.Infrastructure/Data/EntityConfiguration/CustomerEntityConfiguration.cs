using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Data.EntityConfiguration;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customera");

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
