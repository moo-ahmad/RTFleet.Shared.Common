using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RTFleet.Shared.Common.Logging;
using Microsoft.Extensions.Hosting;

namespace RTFleet.Shared.Common.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppLogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, IAppLogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var traceId = context.TraceIdentifier;
                _logger.LogError(ex,
                    $"Unhandled exception.{Environment.NewLine}" +
                    $"TraceId: {traceId}, Path: {context.Request.Path}, Method: {context.Request.Method}{Environment.NewLine}");
                var response = new
                {
                    success = false,
                    statusCode = context.Response.StatusCode,
                    error = new
                    {
                        type = _env.IsDevelopment() ? ex.GetType().Name : null,
                        message = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred. Please try again later.",
                        traceId = traceId,
                        path = _env.IsDevelopment() ? context.Request.Path : null,
                        method = context.Request.Method,
                        timeStamp = DateTime.UtcNow
                    }
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }

        }
    }
}
