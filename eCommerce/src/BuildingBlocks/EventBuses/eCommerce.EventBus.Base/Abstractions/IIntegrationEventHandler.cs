using eCommerce.EventBus.Base.Events;

namespace eCommerce.EventBus.Base.Abstractions;

/// <summary>
/// Interface for handling a specific type of integration event.
/// Implement this interface to define event-handling logic for a given event type.
/// </summary>
/// <typeparam name="TIntegrationEvent">The type of integration event to handle.</typeparam>
public interface IIntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandlerBase
    where TIntegrationEvent : IntegrationEvent
{
    /// <summary>
    /// Asynchronously handles the specified integration event.
    /// </summary>
    /// <param name="event">The event instance to handle.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Handle(TIntegrationEvent @event);
}