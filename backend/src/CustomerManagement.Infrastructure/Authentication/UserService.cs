namespace CustomerManagement.Infrastructure.Authentication;

// In a real app this would query a Users table.
// For this project we use hardcoded users to keep focus on architecture.
public class UserService
{
    private static readonly Dictionary<string, (string Password, string Role)> Users = new()
    {
        { "admin",       ("Admin@123",   "Admin") },
        { "manager",     ("Manager@123", "SalesManager") },
        { "salesrep",    ("Sales@123",   "SalesRep") }
    };

    public (bool success, string role) ValidateUser(string username, string password)
    {
        if (Users.TryGetValue(username.ToLower(), out var user) && user.Password == password)
            return (true, user.Role);

        return (false, string.Empty);
    }
}