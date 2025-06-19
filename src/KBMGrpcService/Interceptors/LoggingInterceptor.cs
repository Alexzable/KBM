using Grpc.Core.Interceptors;
using Grpc.Core;
using Serilog;
using Serilog.Context;
using KBMGrpcService.Common.Constants;

namespace KBMGrpcService.Grpc.Interceptors
{

    public class LoggingInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var traceId = context.RequestHeaders.FirstOrDefault(h => h.Key == AppConstants.TraceHeader)?.Value
                          ?? Guid.NewGuid().ToString();

            using (LogContext.PushProperty("TraceId", traceId))
            {
                Log.Information("GRPC Request: {Method} - TraceId: {TraceId} - {@Request}", context.Method, traceId, request);
                try
                {
                    var response = await continuation(request, context);
                    Log.Information("GRPC Response: {Method} - TraceId: {TraceId} - {@Response}", context.Method, traceId, response);
                    return response;
                }
                catch (RpcException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "GRPC Error: {Method} - TraceId: {TraceId}", context.Method, traceId);
                    throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
                }
            }
        }
    }
}
