using Grpc.Core.Interceptors;
using Grpc.Core;
using Serilog;

namespace KBMGrpcService.Grpc.Interceptors
{
    public class LoggingInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
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
            catch (System.Exception ex)
            {
                Log.Error(ex, "GRPC Error: {Method}", context.Method);
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }
    }
}
