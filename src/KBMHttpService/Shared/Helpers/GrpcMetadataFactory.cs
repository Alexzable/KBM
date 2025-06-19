using Grpc.Core;

namespace KBMHttpService.Shared.Helpers
{
    public class GrpcMetadataFactory(IHttpContextAccessor httpContextAccessor)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private const string TraceIdHeader = "x-trace-id";

        public Metadata Create()
        {
            var traceId = _httpContextAccessor.HttpContext?.Items[TraceIdHeader]?.ToString()
                          ?? Guid.NewGuid().ToString();

            return new Metadata
            {
                { TraceIdHeader, traceId }
            };
        }
    }
}
