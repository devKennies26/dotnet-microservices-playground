using eCommerce.EventBus.Base.Events;

namespace eCommerce.EventBus.Base.Abstractions;

/// <summary>
/// Abstraction for an event bus used in microservice communication.
/// Defines publish-subscribe methods for integration events.
/// </summary>
public interface IEventBus : IDisposable
{
    /// <summary>
    /// Publishes an integration event to the event bus.
    /// All registered handlers for this event type will receive it.
    /// </summary>
    /// <param name="event">The integration event to be published.</param>
    void Publish(IntegrationEvent @event);

    /// <summary>
    /// Subscribes a handler to a specific integration event type.
    /// The handler will be triggered when that event type is published.
    /// </summary>
    /// <typeparam name="TEvent">The type of the integration event.</typeparam>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    void Subscribe<TEvent, TEventHandler>()
        where TEvent : IntegrationEvent
        where TEventHandler : IIntegrationEventHandler<TEvent>;

    /// <summary>
    /// Unsubscribes a handler from a specific integration event type.
    /// The handler will no longer receive events of this type.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <typeparam name="TH">The type of the event handler.</typeparam>
    void UnSubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
}