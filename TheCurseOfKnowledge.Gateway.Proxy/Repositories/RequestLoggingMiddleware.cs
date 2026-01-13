using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCurseOfKnowledge.Gateway.Proxy.Repositories
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                                ?? Guid.NewGuid().ToString();

            var username = context.User?.Identity?.Name ?? "Anonymous";
            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Username", username))
            using (LogContext.PushProperty("UserAgent", context.Request.Headers["User-Agent"].ToString()))
            {
                _logger.LogInformation("Inbound: {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing {Path}",
                        context.Request.Path);
                    throw;
                }

                _logger.LogInformation("Outbound: {Path} responded {StatusCode}",
                    context.Request.Path, context.Response.StatusCode);
            }
        }
    }
}
