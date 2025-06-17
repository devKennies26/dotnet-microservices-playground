using eCommerce.EventBus.Base.Abstractions;
using eCommerce.EventBus.Base.Events;

namespace eCommerce.EventBus.Base.SubManagers;

/// <summary>
/// Manages in-memory subscriptions for events and their corresponding handlers.
/// Provides functionality to add, remove, and query event subscriptions.
/// </summary>
public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
{
    private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
    private readonly List<Type> _eventTypes;

    /// <summary>
    /// Function used to normalize or transform event names (e.g., for versioning or namespacing).
    /// </summary>
    public Func<string, string> eventNameGetter;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryEventBusSubscriptionManager"/> class.
    /// </summary>
    /// <param name="eventNameGetter">A function to transform event names to a consistent format.</param>
    public InMemoryEventBusSubscriptionManager(Func<string, string> eventNameGetter)
    {
        _handlers = new Dictionary<string, List<SubscriptionInfo>>();
        _eventTypes = new List<Type>();
        this.eventNameGetter = eventNameGetter;
    }

    /// <summary>
    /// Gets a value indicating whether there are no subscriptions.
    /// </summary>
    public bool IsEmpty => !_handlers.Keys.Any();

    /// <summary>
    /// Event triggered when all handlers for a specific event have been removed.
    /// </summary>
    public event EventHandler<string> OnEventRemoved;

    /// <summary>
    /// Adds a subscription for a specific event type and its handler type.
    /// </summary>
    public void AddSubscription<TEvent, TEventHandler>()
        where TEvent : IntegrationEvent
        where TEventHandler : IIntegrationEventHandler<TEvent>
    {
        string eventName = GetEventKey<TEvent>();

        AddSubscription(typeof(TEventHandler), eventName);

        if (!_eventTypes.Contains(typeof(TEvent)))
            _eventTypes.Add(typeof(TEvent));
    }

    /// <summary>
    /// Adds a handler type to the internal handler list for the specified event name.
    /// </summary>
    private void AddSubscription(Type handlerType, string eventName)
    {
        if (!HasSubscriptionsForEvent(eventName))
            _handlers.Add(eventName, new List<SubscriptionInfo>());

        if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'",
                nameof(handlerType));

        _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
    }

    /// <summary>
    /// Removes a subscription for a specific event type and its handler.
    /// </summary>
    public void RemoveSubscription<TEvent, TEventHandler>()
        where TEvent : IntegrationEvent
        where TEventHandler : IIntegrationEventHandler<TEvent>
    {
        SubscriptionInfo handlerToRemove = FindSubscriptionToRemove<TEvent, TEventHandler>();
        string eventName = GetEventKey<TEvent>();
        RemoveHandler(eventName, handlerToRemove);
    }

    /// <summary>
    /// Removes a handler from the internal list and triggers the OnEventRemoved event if necessary.
    /// </summary>
    private void RemoveHandler(string eventName, SubscriptionInfo subsToRemove)
    {
        if (subsToRemove is not null)
        {
            _handlers[eventName].Remove(subsToRemove);

            if (!_handlers[eventName].Any())
            {
                _handlers.Remove(eventName);

                Type? eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                if (eventType is not null)
                    _eventTypes.Remove(eventType);

                RaiseOnEventRemoved(eventName);
            }
        }
    }

    /// <summary>
    /// Raises the <see cref="OnEventRemoved"/> event.
    /// </summary>
    private void RaiseOnEventRemoved(string eventName)
    {
        EventHandler<string>? handler = OnEventRemoved;
        handler?.Invoke(this, eventName);
    }

    /// <summary>
    /// Finds a specific subscription to remove based on event and handler type.
    /// </summary>
    private SubscriptionInfo FindSubscriptionToRemove<TEvent, TEventHandler>()
        where TEvent : IntegrationEvent
        where TEventHandler : IIntegrationEventHandler<TEvent>
    {
        string eventName = GetEventKey<TEvent>();
        return FindSubscriptionToRemove(eventName, typeof(TEventHandler));
    }

    /// <summary>
    /// Finds a specific subscription to remove based on event name and handler type.
    /// </summary>
    private SubscriptionInfo FindSubscriptionToRemove(string eventName, Type handlerType)
    {
        if (!HasSubscriptionsForEvent(eventName))
            return null;
        return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
    }

    /// <summary>
    /// Determines whether there are any subscriptions for the specified event type.
    /// </summary>
    public bool HasSubscriptionsForEvent<TEvent>() where TEvent : IntegrationEvent
    {
        string key = GetEventKey<TEvent>();
        return HasSubscriptionsForEvent(key);
    }

    /// <summary>
    /// Determines whether there are any subscriptions for the specified event name.
    /// </summary>
    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

    /// <summary>
    /// Gets the event type from its name.
    /// </summary>
    public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(e => e.Name == eventName);

    /// <summary>
    /// Gets all handler subscriptions for a specified event type.
    /// </summary>
    public IEnumerable<SubscriptionInfo> GetHandlersForEvent<TEvent>() where TEvent : IntegrationEvent
    {
        string key = GetEventKey<TEvent>();
        return GetHandlersForEvent(key);
    }

    /// <summary>
    /// Gets all handler subscriptions for a specified event name.
    /// </summary>
    public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

    /// <summary>
    /// Generates a normalized key for the specified event type using <see cref="eventNameGetter"/>.
    /// </summary>
    public string GetEventKey<TEvent>()
    {
        string eventName = typeof(TEvent).Name;
        return eventNameGetter(eventName);
    }

    /// <summary>
    /// Removes all event subscriptions.
    /// </summary>
    public void Clear() => _handlers.Clear();
}