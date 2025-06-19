using AutoMapper;
using FluentValidation;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace KBMHttpService.Shared.Exceptions
{
    public static class ExceptionUtils
    {
        public static T ExecuteMapping<T>(Func<T> action, ILogger logger)
        {
            try { return action(); }
            catch (AutoMapperMappingException ex)
            {
                logger.LogError(ex, "Mapping error");
                throw new ClientMappingException("Invalid request mapping.", ex);
            }
        }

        public static IActionResult HandleException(Exception ex)
        {
            return ex switch
            {
                ExternalServiceException ese => new ObjectResult(new { error = ese.Message }) { StatusCode = 502 },
                KeyNotFoundException => new NotFoundObjectResult(new { error = ex.Message }),
                ValidationException ve => new BadRequestObjectResult(new { errors = ve.Errors }),
                _ => new ObjectResult(new ErrorResponse("Unexpected server error.")) { StatusCode = 500 }
            };
        }

        public static async Task<IActionResult> TryCatchAsync(
               Func<Task<IActionResult>> action,
               ILogger logger,
               string context)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Context} failed", context);
                return HandleException(ex);
            }
        }

        public static async Task<T> ExecuteGrpcCallAsync<T>(Func<Task<T>> action, string context, ILogger logger)
        {
            try
            {
                return await action();
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "gRPC call failed in {Context}", context);
                throw new ExternalServiceException($"{context} failed.", ex);
            }
        }
    }
}
