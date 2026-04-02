using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Infrastructure.Persistence;
using CustomerManagement.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CustomerManagement.Tests.Infrastructure;

public class ContactRepositoryTests
{
    private CustomerManagementDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<CustomerManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new CustomerManagementDbContext(options);
    }

    // Test 31
    [Fact]
    public async Task AddAsync_ShouldSaveContactToDatabase()
    {
        using var context = CreateInMemoryContext();

        var customer = Customer.Create(
            "John", "Smith", "john@acme.com",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();

        var repo = new ContactRepository(context);

        var contact = Contact.Create(
            customer.Id, "Alice", "Brown",
            "alice@acme.com", true, "9876543210", "Manager");

        await repo.AddAsync(contact);
        await repo.SaveChangesAsync();

        var saved = await context.Contacts
            .FirstOrDefaultAsync(c => c.Id == contact.Id);

        saved.Should().NotBeNull();
        saved!.Email.Should().Be("alice@acme.com");
    }

    // Test 32
    [Fact]
    public async Task HasPrimaryContactAsync_WhenPrimaryExists_ShouldReturnTrue()
    {
        using var context = CreateInMemoryContext();

        var customer = Customer.Create(
            "John", "Smith", "john@acme.com",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();

        var contact = Contact.Create(
            customer.Id, "Alice", "Brown",
            "alice@acme.com", true, null, null);

        await context.Contacts.AddAsync(contact);
        await context.SaveChangesAsync();

        var repo = new ContactRepository(context);
        var hasPrimary = await repo.HasPrimaryContactAsync(customer.Id);

        hasPrimary.Should().BeTrue();
    }

    // Test 33
    [Fact]
    public async Task HasPrimaryContactAsync_WhenNoPrimaryExists_ShouldReturnFalse()
    {
        using var context = CreateInMemoryContext();

        var customer = Customer.Create(
            "John", "Smith", "john@acme.com",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();

        var repo = new ContactRepository(context);
        var hasPrimary = await repo.HasPrimaryContactAsync(customer.Id);

        hasPrimary.Should().BeFalse();
    }

    // Test 34
    [Fact]
    public async Task GetByCustomerIdAsync_ShouldReturnContactsForCustomer()
    {
        using var context = CreateInMemoryContext();

        var customer = Customer.Create(
            "John", "Smith", "john@acme.com",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();

        var contact1 = Contact.Create(
            customer.Id, "Alice", "Brown",
            "alice@acme.com", true, null, null);

        var contact2 = Contact.Create(
            customer.Id, "Bob", "Jones",
            "bob@acme.com", false, null, null);

        await context.Contacts.AddRangeAsync(contact1, contact2);
        await context.SaveChangesAsync();

        var repo = new ContactRepository(context);
        var contacts = await repo.GetByCustomerIdAsync(customer.Id);

        contacts.Should().HaveCount(2);
    }
}