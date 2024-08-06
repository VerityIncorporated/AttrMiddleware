using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AttrMiddleware;

/// <summary>
/// Represents a middleware pipeline that executes a series of middleware components.
/// </summary>
public class MiddlewarePipeline
{
    /// <summary>
    /// Invokes the middleware pipeline.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="middlewares">The collection of middlewares to execute.</param>
    /// <param name="next">The next middleware in the pipeline.</param>
    public static async Task InvokeAsync(HttpContext context, IEnumerable<IMiddleware> middlewares, RequestDelegate next)
    {
        RequestDelegate pipeline = async (ctx) =>
        {
            await next(ctx);
        };
            
        foreach (var middleware in middlewares.Reverse())
        {
            var currentMiddleware = middleware;
            var requestDelegate = pipeline;
            pipeline = async (ctx) =>
            {
                await currentMiddleware.InvokeAsync(ctx, requestDelegate);
            };
        }
            
        await pipeline(context);
    }
}