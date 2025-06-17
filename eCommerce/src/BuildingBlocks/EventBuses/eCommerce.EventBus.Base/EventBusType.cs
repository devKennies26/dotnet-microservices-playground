namespace eCommerce.EventBus.Base;

/// <summary>
/// Enumeration of supported Event Bus types.
/// Used to define which messaging infrastructure is being used.
/// </summary>
public enum EventBusType
{
    /// <summary>
    /// RabbitMQ message broker.
    /// </summary>
    RabbitMQ = 0,

    /// <summary>
    /// Azure Service Bus provided by Microsoft Azure.
    /// </summary>
    AzureServiceBus = 1
}