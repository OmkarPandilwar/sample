using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Domain.Entities;

public class Interaction
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public InteractionType Type { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string Details { get; private set; } = string.Empty;
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime InteractionDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Customer? Customer { get; private set; }

    private Interaction() { }

    public static Interaction Create(
        Guid customerId,
        InteractionType type,
        string subject,
        string details,
        string createdBy,
        DateTime? interactionDate = null)
    {
        if (string.IsNullOrWhiteSpace(subject))
            throw new DomainException("Subject is required.");
        if (string.IsNullOrWhiteSpace(details))
            throw new DomainException("Details are required.");
        if (string.IsNullOrWhiteSpace(createdBy))
            throw new DomainException("CreatedBy is required.");

        return new Interaction
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Type = type,
            Subject = subject.Trim(),
            Details = details.Trim(),
            CreatedBy = createdBy,
            InteractionDate = interactionDate ?? DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
    }
}