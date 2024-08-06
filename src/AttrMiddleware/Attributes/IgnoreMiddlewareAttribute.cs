namespace AttrMiddleware.Attributes;

/// <summary>
/// An attribute used to specify that certain middleware should be ignored.
/// </summary>
/// <param name="type">The type of middleware to ignore.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class IgnoreMiddlewareAttribute(Type type = null!) : Attribute
{
    /// <summary>
    /// Gets the type of middleware to ignore.
    /// </summary>
    public Type Type { get; } = type;
}