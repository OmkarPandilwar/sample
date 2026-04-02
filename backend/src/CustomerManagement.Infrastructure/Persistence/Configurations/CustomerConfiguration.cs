using CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerManagement.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CustomerName)
            .IsRequired().HasMaxLength(200);
        builder.Property(c => c.Email)
            .IsRequired().HasMaxLength(200);
        builder.Property(c => c.Phone).HasMaxLength(20);
        builder.Property(c => c.Website).HasMaxLength(300);
        builder.Property(c => c.Industry).HasMaxLength(100);
        builder.Property(c => c.CompanySize).HasMaxLength(50);
        builder.Property(c => c.Classification).IsRequired();
        builder.Property(c => c.Type).IsRequired();
        builder.Property(c => c.Segment).IsRequired();
        builder.Property(c => c.AccountValue)
            .HasColumnType("decimal(18,2)");
        builder.Property(c => c.AssignedSalesRepId).HasMaxLength(100);

        builder.HasIndex(c => c.Email).IsUnique();
        builder.HasIndex(c => c.Segment);
        builder.HasIndex(c => c.Classification);
        builder.HasIndex(c => c.IsActive);

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