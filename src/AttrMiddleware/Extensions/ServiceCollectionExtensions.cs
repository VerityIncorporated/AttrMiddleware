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
    public static void RegisterAttrMiddleware<TMiddleware>(this IServiceCollection services)
    {
        services.AddSingleton(typeof(TMiddleware));
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
}