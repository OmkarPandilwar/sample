using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Application.Commands.Interactions;

public class LogInteractionHandler
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly ICustomerRepository _customerRepository;

    public LogInteractionHandler(IInteractionRepository interactionRepository, ICustomerRepository customerRepository)
    {
        _interactionRepository = interactionRepository;
        _customerRepository = customerRepository;
    }

    public async Task<InteractionDto> HandleAsync(LogInteractionCommand command, CancellationToken ct = default)
    {
        _ = await _customerRepository.GetByIdAsync(command.CustomerId, ct)
            ?? throw new CustomerNotFoundException(command.CustomerId);

        var interaction = Interaction.Create(
            command.CustomerId,
            command.Type,
            command.Notes,
            command.CreatedBy,
            command.InteractionDate);

        await _interactionRepository.AddAsync(interaction, ct);
        await _interactionRepository.SaveChangesAsync(ct);

        return new InteractionDto(
            interaction.Id, interaction.CustomerId,
            interaction.Type, interaction.Notes,
            interaction.CreatedBy, interaction.InteractionDate,
            interaction.CreatedAt);
    }
}