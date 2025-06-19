using Grpc.Core.Interceptors;
using Grpc.Core;
using Serilog;
using Serilog.Context;
using KBMGrpcService.Shared.Constants;
using static KBMGrpcService.Shared.Exceptions.GrpcCustomError;

namespace KBMGrpcService.Shared.Interceptors
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
                TraceContext.TraceId = traceId;

                Log.Information("GRPC Request: {Method} - {@Request}", context.Method, request);

                try
                {
                    var response = await continuation(request, context);
                    Log.Information("GRPC Response: {Method} - {@Response}", context.Method, response);
                    return response;
                }
                catch (RpcException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Log.Error("‼️ Unexpected error in {Method} - Reason: {Message} - TraceId: {TraceId}",
                        context.Method, ex.Message, traceId);
                    throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
                }
            }
        }
    }
}
