using AutoMapper;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Serilog;

namespace KBMGrpcService.Common.Exceptions
{
    public static class GrpcCustomError
    {
        public static RpcException FromException(Exception ex)
        {
            return ex switch
            {
                AutoMapperMappingException =>
                    new RpcException(new Status(StatusCode.Internal, "Mapping error: " + ex.Message)),

                KeyNotFoundException =>
                    new RpcException(new Status(StatusCode.NotFound, ex.Message)),

                SqlException sqlEx when sqlEx.Number is 2601 or 2627 => HandleSqlDuplicateException(sqlEx),

                SqlException sqlEx =>
                    new RpcException(new Status(StatusCode.Internal, "Database error: " + sqlEx.Message)),

                ArgumentException argEx =>
                    new RpcException(new Status(StatusCode.InvalidArgument, argEx.Message)),

                _ =>
                    new RpcException(new Status(StatusCode.Unknown, "Unexpected error: " + ex.Message))
            };
        }

        private static RpcException HandleSqlDuplicateException(SqlException sqlEx)
        {
            var message = sqlEx.Message.ToLowerInvariant();

            if (message.Contains("name"))
                return new RpcException(new Status(StatusCode.InvalidArgument, "An entity with this name already exists."));

            if (message.Contains("email"))
                return new RpcException(new Status(StatusCode.InvalidArgument, "An entity with this email already exists."));

            return new RpcException(new Status(StatusCode.InvalidArgument, "Duplicate data error: one or more unique constraints were violated."));
        }

        public static async Task<T> TryCatchAsync<T>(
            Func<Task<T>> action,
            string context,
            object? contextData = null)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "gRPC error in {Context} {@Data}", context, contextData);
                throw FromException(ex);
            }
        }

        public static async Task TryCatchAsync(
            Func<Task> action,
            string context,
            object? contextData = null)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "gRPC error in {Context} {@Data}", context, contextData);
                throw FromException(ex);
            }
        }
    }
}
