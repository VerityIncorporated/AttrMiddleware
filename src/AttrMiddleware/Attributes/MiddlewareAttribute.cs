namespace AttrMiddleware.Attributes;

/// <summary>
/// An attribute used to specify a type of middleware.
/// </summary>
/// <param name="type">The type of middleware.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class MiddlewareAttribute(Type type) : Attribute
{
    /// <summary>
    /// Gets the type of middleware.
    /// </summary>
    public Type Type { get; } = type;
}