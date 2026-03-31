using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Application.Commands.Customers;

public class DeleteCustomerHandler
{
    private readonly ICustomerRepository _repository;

    public DeleteCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(DeleteCustomerCommand command, CancellationToken ct = default)
    {
        var customer = await _repository.GetByIdWithDetailsAsync(command.Id, ct)
            ?? throw new CustomerNotFoundException(command.Id);

        customer.Deactivate();
        _repository.Update(customer);
        await _repository.SaveChangesAsync(ct);
    }
}