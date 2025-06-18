using AutoMapper;
using Grpc.Core;
using Microsoft.Data.SqlClient;

namespace KBMGrpcService.Common.Exceptions
{
    public static class GrpcErrorCustom
    {
        public static RpcException FromException(Exception ex)
        {
            return ex switch
            {
                AutoMapperMappingException =>
                    new RpcException(new Status(StatusCode.Internal, "Mapping error: " + ex.Message)),

                KeyNotFoundException =>
                    new RpcException(new Status(StatusCode.NotFound, ex.Message)),

                SqlException sqlEx =>
                    new RpcException(new Status(StatusCode.Internal, "Database error: " + sqlEx.Message)),

                ArgumentException argEx =>
                    new RpcException(new Status(StatusCode.InvalidArgument, argEx.Message)),

                _ =>
                    new RpcException(new Status(StatusCode.Unknown, "Unexpected error: " + ex.Message))
            };
        }
    }
}
