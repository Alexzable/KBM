using Serilog.Context;

namespace KBMHttpService.Shared.Middleware
{
    public class TraceIdMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private const string TraceIdHeader = "x-trace-id";

        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = context.Request.Headers[TraceIdHeader].FirstOrDefault() ?? Guid.NewGuid().ToString();
            context.Items[TraceIdHeader] = traceId;

            using (LogContext.PushProperty("TraceId", traceId))
            {
                await _next(context);
            }
        }
    }
}
