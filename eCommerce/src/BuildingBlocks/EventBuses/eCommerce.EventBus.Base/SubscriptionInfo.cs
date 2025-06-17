namespace eCommerce.EventBus.Base;

/// <summary>
/// Represents information about a subscription to an integration event.
/// Holds metadata about the event handler type that processes the event.
/// </summary>
public class SubscriptionInfo
{
    /// <summary>
    /// Gets the type of the event handler associated with the subscription.
    /// </summary>
    public Type HandlerType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionInfo"/> class
    /// with the specified event handler type.
    /// </summary>
    /// <param name="handlerType">The type of the event handler.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="handlerType"/> is null.
    /// </exception>
    public SubscriptionInfo(Type handlerType)
    {
        HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
    }

    /// <summary>
    /// Creates a new instance of <see cref="SubscriptionInfo"/> for the given handler type.
    /// </summary>
    /// <param name="handlerType">The type of the event handler.</param>
    /// <returns>A new <see cref="SubscriptionInfo"/> instance.</returns>
    public static SubscriptionInfo Typed(Type handlerType)
    {
        return new SubscriptionInfo(handlerType);
    }
}