namespace CustomerManagement.Domain.Exceptions;

public class CustomerNotFoundException : DomainException
{
    public CustomerNotFoundException(Guid id)
        : base($"Customer with ID '{id}' was not found.") { }
}