using EfCustomerOrdersMySql;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        using var db = new AppDbContext();

        // -----------------------------
        // INSERT CUSTOMER
        // -----------------------------
             var customer = new Customer
        {
            Name = "Omkar"
        };

        db.Customers.Add(customer);
        db.SaveChanges();   // Customer Id generated here

        Console.WriteLine($"Customer inserted with Id: {customer.Id}");

        // -----------------------------
        // INSERT MULTIPLE ORDERS
        // -----------------------------
        var orders = new List<Order>
        {
            new Order { Product = "Mouse", Price = 499, CustomerId = customer.Id },
            new Order { Product = "Keyboard", Price = 999, CustomerId = customer.Id },
            new Order { Product = "Monitor", Price = 7500, CustomerId = customer.Id }
        };

        db.Orders.AddRange(orders);
        db.SaveChanges();

        Console.WriteLine("Orders inserted successfully.");

        // -----------------------------
        // FETCH ORDERS FOR CUSTOMER
        // -----------------------------
        var customerOrders = db.Orders
                               .Where(o => o.CustomerId == customer.Id)
                               .ToList();

        Console.WriteLine("Orders for this customer:");

        foreach (var order in customerOrders)
        {
            Console.WriteLine($"OrderId: {order.Id} | Product: {order.Product} | Price: {order.Price}");
        }
    }
}
