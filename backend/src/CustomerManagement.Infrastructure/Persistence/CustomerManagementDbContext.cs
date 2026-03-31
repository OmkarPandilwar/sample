using CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Infrastructure.Persistence;

public class CustomerManagementDbContext : DbContext
{
    public CustomerManagementDbContext(DbContextOptions<CustomerManagementDbContext> options)
        : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Interaction> Interactions => Set<Interaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerManagementDbContext).Assembly);
    }
}