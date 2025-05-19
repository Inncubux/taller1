using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Middleware for global exception handling.
/// Catches unhandled exceptions, logs them, and returns a standardized error response.
/// </summary>
namespace ECommerce.src.Middleware
{
    public class ExceptionMiddleware(IHostEnvironment env, ILogger<ExceptionMiddleware> logger) : IMiddleware
    {
        /// <summary>
        /// Invokes the middleware to handle exceptions during the HTTP request pipeline.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="next">The next middleware in the pipeline.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        /// <summary>
        /// Handles the exception by logging it and writing a standardized JSON error response.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="ex">The exception that was thrown.</param>
        private async Task HandleException(HttpContext context, Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ProblemDetails
            {
                Status = 500,
                Detail = env.IsDevelopment()
                    ? ex.StackTrace?.ToString()
                    : null,
                Title = ex.Message,
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}