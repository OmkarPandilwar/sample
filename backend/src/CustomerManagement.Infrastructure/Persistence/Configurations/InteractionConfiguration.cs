using CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerManagement.Infrastructure.Persistence.Configurations;

public class InteractionConfiguration : IEntityTypeConfiguration<Interaction>
{
    public void Configure(EntityTypeBuilder<Interaction> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Notes).IsRequired().HasMaxLength(2000);
        builder.Property(i => i.CreatedBy).IsRequired().HasMaxLength(100);
        builder.Property(i => i.Type).IsRequired();

        builder.HasIndex(i => i.CustomerId);
        builder.HasIndex(i => i.InteractionDate);

        builder.ToTable("Interactions");
    }
}