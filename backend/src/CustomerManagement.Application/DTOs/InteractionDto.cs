using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.DTOs;

public record InteractionDto(
    Guid Id,
    Guid CustomerId,
    InteractionType Type,
    string Notes,
    string CreatedBy,
    DateTime InteractionDate,
    DateTime CreatedAt
);

public record CreateInteractionRequest(
    Guid CustomerId,
    InteractionType Type,
    string Notes,
    DateTime? InteractionDate
);