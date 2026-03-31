using CustomerManagement.Application.Commands.Contacts;
using CustomerManagement.Application.Commands.Customers;
using CustomerManagement.Application.Commands.Interactions;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Application.Queries.Analytics;
using CustomerManagement.Application.Queries.Contacts;
using CustomerManagement.Application.Queries.Customers;
using CustomerManagement.Infrastructure.Authentication;
using CustomerManagement.Infrastructure.Caching;
using CustomerManagement.Infrastructure.Persistence;
using CustomerManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core + SQL Server
        services.AddDbContext<CustomerManagementDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("CustomerManagement.Infrastructure")));

        // Redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "CustomerMgmt_";
        });

        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IInteractionRepository, InteractionRepository>();

        // Analytics (Redis-cached)
        services.AddScoped<IAnalyticsService, AnalyticsCacheService>();

        // CQRS Handlers — Commands
        services.AddScoped<CreateCustomerHandler>();
        services.AddScoped<UpdateCustomerHandler>();
        services.AddScoped<DeleteCustomerHandler>();
        services.AddScoped<AddContactHandler>();
        services.AddScoped<LogInteractionHandler>();

        // CQRS Handlers — Queries
        services.AddScoped<GetAllCustomersHandler>();
        services.AddScoped<GetCustomerByIdHandler>();
        services.AddScoped<GetContactsByCustomerHandler>();
        services.AddScoped<GetCustomerAnalyticsHandler>();

        // Auth
        services.AddScoped<JwtTokenService>();
        services.AddScoped<UserService>();

        return services;
    }
}