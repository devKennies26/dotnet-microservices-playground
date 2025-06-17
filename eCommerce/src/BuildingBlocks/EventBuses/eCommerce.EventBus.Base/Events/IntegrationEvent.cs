using Newtonsoft.Json;

namespace eCommerce.EventBus.Base.Events;

/// <summary>
/// Base class for integration events used in microservice communication.
/// Provides common properties like unique Id and creation timestamp for tracking and consistency.
/// </summary>
public class IntegrationEvent
{
    /// <summary>
    /// Unique identifier for the event.
    /// Useful for event tracking and ensuring idempotency across distributed systems.
    /// </summary>
    [JsonProperty]
    public Guid Id { get; private set; }

    /// <summary>
    /// The timestamp indicating when the event was created.
    /// Helps with debugging, logging, and ensuring correct event ordering.
    /// </summary>
    [JsonProperty]
    public DateTime CreatedDate { get; private set; }

    /// <summary>
    /// Default constructor used when creating a new event to be published.
    /// Automatically sets a new GUID and the current time as the creation timestamp.
    /// </summary>
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.Now;
    }

    /// <summary>
    /// Constructor used during deserialization, e.g., when receiving the event from a message broker.
    /// Ensures the original event ID and timestamp are preserved.
    /// </summary>
    /// <param name="id">The unique identifier of the event.</param>
    /// <param name="createdDate">The original creation time of the event.</param>
    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime createdDate)
    {
        Id = id;
        CreatedDate = createdDate;
    }
}