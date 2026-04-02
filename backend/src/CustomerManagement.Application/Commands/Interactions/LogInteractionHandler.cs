using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomerManagement.Application.Commands.Interactions;

public class LogInteractionHandler
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IDistributedCache _cache;
    private readonly IValidator<LogInteractionCommand> _validator;

    public LogInteractionHandler(
        IInteractionRepository interactionRepository,
        ICustomerRepository customerRepository,
        IDistributedCache cache,
        IValidator<LogInteractionCommand> validator)
    {
        _interactionRepository = interactionRepository;
        _customerRepository = customerRepository;
        _cache = cache;
        _validator = validator;
    }

    public async Task<InteractionDto> HandleAsync(
        LogInteractionCommand command,
        CancellationToken ct = default)
    {
        await _validator.ValidateAndThrowAsync(command, ct);

        _ = await _customerRepository.GetByIdAsync(command.CustomerId, ct)
            ?? throw new CustomerNotFoundException(command.CustomerId);

        var interaction = Interaction.Create(
            command.CustomerId,
            command.Type,
            command.Subject,
            command.Details,
            command.CreatedBy,
            command.InteractionDate);

        await _interactionRepository.AddAsync(interaction, ct);
        await _interactionRepository.SaveChangesAsync(ct);

        // Invalidate analytics cache
        await InvalidateCacheAsync(ct);

        return new InteractionDto(
            interaction.Id, interaction.CustomerId,
            interaction.Type, interaction.Subject,
            interaction.Details, interaction.CreatedBy,
            interaction.InteractionDate, interaction.CreatedAt);
    }

    private async Task InvalidateCacheAsync(CancellationToken ct)
    {
        var keys = new[] { "analytics_overview", "analytics_lifetime", "analytics_health", "analytics_segmentation", "analytics_churn" };
        foreach (var key in keys)
            await _cache.RemoveAsync(key, ct);
    }
}