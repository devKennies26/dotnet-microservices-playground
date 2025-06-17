using eCommerce.EventBus.Base.Abstractions;
using eCommerce.EventBus.Base.SubManagers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace eCommerce.EventBus.Base.Events;

/// <summary>
/// Provides an abstract base implementation for an event bus that supports 
/// publishing, subscribing, and processing integration events.
/// </summary>
public abstract class BaseEventBus : IEventBus
{
    /// <summary>
    /// Gets the service provider for resolving dependencies.
    /// </summary>
    public readonly IServiceProvider ServiceProvider;

    /// <summary>
    /// Gets the subscription manager responsible for managing event subscriptions.
    /// </summary>
    public readonly IEventBusSubscriptionManager SubscriptionManager;

    /// <summary>
    /// Gets or sets the event bus configuration.
    /// </summary>
    public EventBusConfig EventBusConfig { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEventBus"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    /// <param name="eventBusConfig">The configuration settings for the event bus.</param>
    protected BaseEventBus(IServiceProvider serviceProvider, EventBusConfig eventBusConfig)
    {
        EventBusConfig = eventBusConfig;
        ServiceProvider = serviceProvider;
        SubscriptionManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
    }

    /// <summary>
    /// Processes the event name by removing the configured prefix and/or suffix if specified.
    /// </summary>
    /// <param name="eventName">The original event name.</param>
    /// <returns>The processed event name.</returns>
    public virtual string ProcessEventName(string eventName)
    {
        if (EventBusConfig.DeleteEventPrefix)
            eventName = eventName.TrimStart(EventBusConfig.EventNamePrefix.ToArray());

        if (EventBusConfig.DeleteEventSuffix)
            eventName = eventName.TrimEnd(EventBusConfig.EventNameSuffix.ToArray());

        return eventName;
    }

    /// <summary>
    /// Constructs the subscriber name using the client application name and event name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns>The generated subscriber name.</returns>
    public virtual string GetSubName(string eventName)
    {
        return $"{EventBusConfig.SubscriberClientAppName}.{ProcessEventName(eventName)}";
    }

    /// <summary>
    /// Releases resources and clears subscriptions.
    /// </summary>
    public virtual void Dispose()
    {
        EventBusConfig = null;
        SubscriptionManager.Clear();
    }

    /// <summary>
    /// Processes an incoming event by invoking the registered handlers.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <param name="message">The serialized event message.</param>
    /// <returns>True if the event was processed by at least one handler; otherwise, false.</returns>
    public async Task<bool> ProcessEvent(string eventName, string message)
    {
        eventName = ProcessEventName(eventName);

        bool processed = false;

        if (SubscriptionManager.HasSubscriptionsForEvent(eventName))
        {
            IEnumerable<SubscriptionInfo> subscriptions = SubscriptionManager.GetHandlersForEvent(eventName);

            using IServiceScope scope = ServiceProvider.CreateScope();

            foreach (SubscriptionInfo subscription in subscriptions)
            {
                object? handler = ServiceProvider.GetService(subscription.HandlerType);
                if (handler is null) continue;

                Type eventType =
                    SubscriptionManager.GetEventTypeByName(
                        $"{EventBusConfig.EventNamePrefix}{eventName}{EventBusConfig.EventNameSuffix}");
                object? integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                Type concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
            }

            processed = true;
        }

        return processed;
    }

    /// <summary>
    /// Publishes an integration event to the event bus.
    /// </summary>
    /// <param name="event">The integration event to publish.</param>
    public abstract void Publish(IntegrationEvent @event);

    /// <summary>
    /// Subscribes the specified event handler to the specified integration event.
    /// </summary>
    /// <typeparam name="TEvent">The type of the integration event.</typeparam>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    public abstract void Subscribe<TEvent, TEventHandler>()
        where TEvent : IntegrationEvent
        where TEventHandler : IIntegrationEventHandler<TEvent>;

    /// <summary>
    /// Unsubscribes the specified event handler from the specified integration event.
    /// </summary>
    /// <typeparam name="TEvent">The type of the integration event.</typeparam>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    public abstract void UnSubscribe<TEvent, TEventHandler>()
        where TEvent : IntegrationEvent
        where TEventHandler : IIntegrationEventHandler<TEvent>;
}