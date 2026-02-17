using Microsoft.EntityFrameworkCore;

namespace EfCustomerOrdersMySql;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connStr = "server=localhost;port=3306;database=ShopDb;user=root;password=root;";
        optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId);
    }
}
