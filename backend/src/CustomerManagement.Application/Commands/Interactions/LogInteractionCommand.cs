using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.Commands.Interactions;

public record LogInteractionCommand(
    Guid CustomerId,
    InteractionType Type,
    string Subject,
    string Details,
    string CreatedBy,
    DateTime? InteractionDate
);