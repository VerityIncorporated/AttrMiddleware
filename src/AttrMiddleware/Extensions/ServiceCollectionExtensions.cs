using System.Reflection;
using AttrMiddleware.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AttrMiddleware.Extensions;

/// <summary>
/// Extension methods for registering AttrMiddleware in the service collection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a specific Middleware type in the service collection.
    /// </summary>
    /// <typeparam name="TMiddleware">The type of middleware to register.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="type">The lifetime of the service to register.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid service type is specified.</exception>
    public static void RegisterAttrMiddleware<TMiddleware>(this IServiceCollection services, ServiceType type = ServiceType.Scoped)
    {
        switch (type)
        {
            case ServiceType.Scoped:
                services.AddScoped(typeof(TMiddleware));
                break;
            case ServiceType.Transient:
                services.AddTransient(typeof(TMiddleware));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid service type specified.");
        }
    }

    /// <summary>
    /// Registers all AttrMiddleware types found in the calling assembly.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void RegisterAllAttrMiddleware(this IServiceCollection services)
    {
        foreach (var type in Assembly.GetCallingAssembly().GetTypes().OrderBy(x => x.Name))
        {
            var attributes = type.GetCustomAttributes<MiddlewareAttribute>();
            foreach (var attrMiddleware in attributes)
            {
                if (services.Any(descriptor => descriptor.ServiceType == attrMiddleware.Type))
                    continue;
                
                services.AddSingleton(attrMiddleware.Type);
            }
        }
    }

    /// <summary>
    /// Enum for specifying the service lifetime.
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// The middleware is created once per request (Scoped).
        /// </summary>
        Scoped,

        /// <summary>
        /// The middleware is created each time it is requested (Transient).
        /// </summary>
        Transient
    }
}