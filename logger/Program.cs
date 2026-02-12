using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

services.AddLogging(b =>
{
    b.ClearProviders();
    b.AddConsole();
    b.SetMinimumLevel(LogLevel.Debug);
});

services.AddScoped<UserService>();

using var sp = services.BuildServiceProvider();
var userService = sp.GetRequiredService<UserService>();
userService.CreateUser("John");

public class UserService
{
    private readonly ILogger<UserService> _logger;

    public UserService(ILogger<UserService> logger) => _logger = logger;

    public void CreateUser(string name)
    {
        _logger.LogInformation("Creating user: {Name}", name);
    }
}
