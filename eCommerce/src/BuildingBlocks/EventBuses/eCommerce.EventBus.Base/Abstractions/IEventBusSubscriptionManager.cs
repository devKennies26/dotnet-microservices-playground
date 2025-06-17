using eCommerce.EventBus.Base.Events;

namespace eCommerce.EventBus.Base.Abstractions;

/// <summary>
/// Defines a contract for managing subscriptions to integration events.
/// Responsible for adding, removing, and querying event-handler mappings.
/// </summary>
public interface IEventBusSubscriptionManager
{
    /// <summary>
    /// Gets a value indicating whether the subscription manager has no registered event handlers.
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Occurs when a subscription to an event is removed.
    /// </summary>
    event EventHandler<string> OnEventRemoved;

    /// <summary>
    /// Adds a subscription for the specified event type and handler type.
    /// </summary>
    /// <typeparam name="TEvent">The type of the integration event.</typeparam>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    void AddSubscription<TEvent, TEventHandler>()
        where TEvent : IntegrationEvent
        where TEventHandler : IIntegrationEventHandler<TEvent>;

    /// <summary>
    /// Removes a subscription for the specified event type and handler type.
    /// </summary>
    /// <typeparam name="TEvent">The type of the integration event.</typeparam>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    void RemoveSubscription<TEvent, TEventHandler>()
        where TEvent : IntegrationEvent
        where TEventHandler : IIntegrationEventHandler<TEvent>;

    /// <summary>
    /// Determines whether there are any handlers subscribed for the specified event type.
    /// </summary>
    /// <typeparam name="TEvent">The type of the integration event.</typeparam>
    /// <returns><c>true</c> if there are subscriptions; otherwise, <c>false</c>.</returns>
    bool HasSubscriptionsForEvent<TEvent>()
        where TEvent : IntegrationEvent;

    /// <summary>
    /// Determines whether there are any handlers subscribed for the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns><c>true</c> if there are subscriptions; otherwise, <c>false</c>.</returns>
    bool HasSubscriptionsForEvent(string eventName);

    /// <summary>
    /// Gets the event type associated with the given event name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns>The event <see cref="Type"/>.</returns>
    Type GetEventTypeByName(string eventName);

    /// <summary>
    /// Gets all handler subscriptions for the specified event type.
    /// </summary>
    /// <typeparam name="TEvent">The type of the integration event.</typeparam>
    /// <returns>A collection of <see cref="SubscriptionInfo"/> for the event.</returns>
    IEnumerable<SubscriptionInfo> GetHandlersForEvent<TEvent>()
        where TEvent : IntegrationEvent;

    /// <summary>
    /// Gets all handler subscriptions for the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns>A collection of <see cref="SubscriptionInfo"/> for the event.</returns>
    IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

    /// <summary>
    /// Gets the unique key (typically the name) used to identify the specified event type.
    /// </summary>
    /// <typeparam name="TEvent">The type of the integration event.</typeparam>
    /// <returns>The event key as a string.</returns>
    string GetEventKey<TEvent>();

    /// <summary>
    /// Removes all event subscriptions from the manager.
    /// </summary>
    void Clear();
}