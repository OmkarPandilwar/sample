using CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerManagement.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Phone).HasMaxLength(20);
        builder.Property(c => c.CompanyName).HasMaxLength(200);
        builder.Property(c => c.Segment).IsRequired();
        builder.Property(c => c.Classification).IsRequired();

        // Indexes for fast query
        builder.HasIndex(c => c.Email).IsUnique();
        builder.HasIndex(c => c.Segment);
        builder.HasIndex(c => c.Classification);
        builder.HasIndex(c => c.IsActive);

        // Relationships
        builder.HasMany(c => c.Contacts)
            .WithOne(c => c.Customer)
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Addresses)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Interactions)
            .WithOne(i => i.Customer)
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Customers");
    }
}