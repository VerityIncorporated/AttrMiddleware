using AttrMiddleware.Attributes;
using Microsoft.AspNetCore.Builder;

namespace AttrMiddleware.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring endpoint conventions with middleware.
    /// </summary>
    public static class EndpointConventionBuilderExtensions
    {
        /// <summary>
        /// Adds a middleware attribute to an endpoint builder.
        /// </summary>
        /// <param name="builder">The endpoint convention builder to which the middleware attribute will be added.</param>
        /// <param name="middleware">The type of middleware to add as an attribute.</param>
        /// <typeparam name="TBuilder">The type of the endpoint convention builder.</typeparam>
        /// <returns>The original endpoint convention builder, allowing for method chaining.</returns>
        public static TBuilder WithMiddleware<TBuilder>(this TBuilder builder, Type middleware) where TBuilder : IEndpointConventionBuilder
        {
            builder.Add(endpointBuilder =>
            {
                var middlewareAttribute = new MiddlewareAttribute(middleware);
                endpointBuilder.Metadata.Add(middlewareAttribute);
            });

            return builder;
        }
    }
}