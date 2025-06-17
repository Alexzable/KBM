using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace KBMHttpService.Common.Exceptions
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
    }
}
