using AttrMiddleware.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace AttrMiddleware.Middlewares;

/// <summary>
/// A middleware executor that handles the invocation of middleware based on attributes.
/// </summary>
/// <param name="next">The next middleware in the pipeline.</param>
public class MiddlewareExecutor(RequestDelegate next)
{
    /// <summary>
    /// Invokes the middleware executor.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        var middlewareAttributes = endpoint?.Metadata.GetOrderedMetadata<MiddlewareAttribute>();
        var ignoreMiddlewareAttributes = endpoint?.Metadata.GetOrderedMetadata<IgnoreMiddlewareAttribute>();
        
        if (middlewareAttributes == null || !middlewareAttributes.Any())
        {
            await next(context);
            return;
        }
        
        var ignoreAll = ignoreMiddlewareAttributes != null && ignoreMiddlewareAttributes.Any(attr => attr.Type == null!);
        
        var middlewares = new List<IMiddleware>();
        foreach (var middlewareAttribute in middlewareAttributes)
        {
            if (context.RequestServices.GetService(middlewareAttribute.Type) is not IMiddleware instance) continue;
            
            if (ignoreAll || (ignoreMiddlewareAttributes != null && ignoreMiddlewareAttributes.Any(attr => attr.Type == middlewareAttribute.Type)))
            {
                continue;
            }
            
            middlewares.Add(instance);
        }
        
        await MiddlewarePipeline.InvokeAsync(context, middlewares, next);
    }
}