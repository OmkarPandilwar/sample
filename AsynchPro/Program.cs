using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var serilogLogger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            services.AddLogging(lb =>
            {
                lb.ClearProviders();
                lb.AddSerilog(serilogLogger, dispose: true);
                lb.SetMinimumLevel(LogLevel.Debug);
            });

            services.AddScoped<UserService>();

            using var serviceProvider = services.BuildServiceProvider();

            var userService = serviceProvider.GetRequiredService<UserService>();
            userService.CreateUser("John");
        }
    }

    public class UserService
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger) => _logger = logger;

        public void CreateUser(string name)
        {
            _logger.LogInformation("Creating user: {Name}", name);

            try
            {
                _logger.LogInformation("User {Name} created successfully", name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user: {Name}", name);
            }
        }
    }
}
