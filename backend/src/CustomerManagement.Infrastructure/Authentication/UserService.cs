namespace CustomerManagement.Infrastructure.Authentication;

// In a real app this would query a Users table.
// For this project we use hardcoded users to keep focus on architecture.
public class UserService
{
    private record UserRecord(string Id, string Password, string Role, string? AssignedRepId = null);

    private static readonly Dictionary<string, UserRecord> Users = new()
    {
        { "admin",    new UserRecord("1", "admin",       "Admin") },
        { "manager",  new UserRecord("2", "Manager@123", "SalesManager") },
        { "salesrep", new UserRecord("3", "Sales@123",   "SalesRep", "REP_001") }
    };

    public (bool success, string role, string userId, string? assignedId) ValidateUser(string username, string password)
    {
        if (Users.TryGetValue(username.ToLower(), out var user) && 
            user.Password.Equals(password, StringComparison.OrdinalIgnoreCase))
            return (true, user.Role, user.Id, user.AssignedRepId);

        return (false, string.Empty, string.Empty, null);
    }
}