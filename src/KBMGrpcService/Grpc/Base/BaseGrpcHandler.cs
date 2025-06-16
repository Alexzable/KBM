namespace KBMGrpcService.Grpc.Base
{
    public class BaseGrpcHandler
    {
        protected readonly ILogger _logger;

        protected BaseGrpcHandler(ILogger logger)
        {
            _logger = logger;
        }
    }
}
