using AttrMiddleware.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace AttrMiddleware.Extensions;

/// <summary>
/// Extension methods for adding AttrMiddleware to the application pipeline.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Adds the <see cref="MiddlewareExecutor"/> middleware to the application's request pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    public static void UseAttrMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<MiddlewareExecutor>();
    }
}