namespace eCommerce.EventBus.Base;

/// <summary>
/// Configuration settings for the Event Bus infrastructure.
/// Used to control connection details, naming conventions, and Event Bus type.
/// </summary>
public class EventBusConfig
{
    /// <summary>
    /// Number of times to retry the connection if the initial attempt fails.
    /// Default is 5.
    /// </summary>
    public int ConnectionRetryCount { get; set; } = 5;

    /// <summary>
    /// Default topic or exchange name used by the Event Bus.
    /// </summary>
    public string DefaultTopicName { get; set; } = "eCommerceEventBus";

    /// <summary>
    /// The connection string used to connect to the Event Bus provider (e.g., RabbitMQ, Azure Service Bus).
    /// </summary>
    public string EventBusConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Name of the subscriber application that listens to the Event Bus.
    /// Useful for logging or distinguishing between multiple clients.
    /// </summary>
    public string SubscriberClientAppName { get; set; } = string.Empty;

    /// <summary>
    /// Optional prefix added to event names (for filtering or naming conventions).
    /// </summary>
    public string EventNamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// Optional suffix added to event names (commonly "IntegrationEvent").
    /// </summary>
    public string EventNameSuffix { get; set; } = "IntegrationEvent";

    /// <summary>
    /// The type of Event Bus to use (e.g., RabbitMQ, AzureServiceBus).
    /// </summary>
    public EventBusType EventBusType { get; set; } = EventBusType.RabbitMQ;

    /// <summary>
    /// Holds a reference to the actual connection object (e.g., IConnection or ServiceBusClient).
    /// This is an abstraction to allow various providers.
    /// </summary>
    public object Connection { get; set; }

    /// <summary>
    /// Indicates whether the event name prefix should be removed when processing events.
    /// </summary>
    public bool DeleteEventPrefix => !string.IsNullOrEmpty(EventNamePrefix);

    /// <summary>
    /// Indicates whether the event name suffix should be removed when processing events.
    /// </summary>
    public bool DeleteEventSuffix => !string.IsNullOrEmpty(EventNameSuffix);
}