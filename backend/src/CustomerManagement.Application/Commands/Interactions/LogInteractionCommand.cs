using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.Commands.Interactions;

public record LogInteractionCommand(
    Guid CustomerId,
    InteractionType Type,
    string Notes,
    string CreatedBy,
    DateTime? InteractionDate
);