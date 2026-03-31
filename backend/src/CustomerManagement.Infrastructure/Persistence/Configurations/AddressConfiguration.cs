using CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerManagement.Infrastructure.Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Street).IsRequired().HasMaxLength(300);
        builder.Property(a => a.City).IsRequired().HasMaxLength(100);
        builder.Property(a => a.State).HasMaxLength(100);
        builder.Property(a => a.PostalCode).HasMaxLength(20);
        builder.Property(a => a.Country).IsRequired().HasMaxLength(100);

        builder.HasIndex(a => a.CustomerId);

        builder.ToTable("Addresses");
    }
}