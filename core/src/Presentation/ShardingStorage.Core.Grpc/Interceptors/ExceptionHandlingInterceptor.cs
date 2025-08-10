using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using ShardingStorage.Core.Domain.Exceptions;

namespace ShardingStorage.Core.Grpc.Interceptors;

internal class ExceptionHandlingInterceptor : Interceptor
{
    private readonly ILogger<ExceptionHandlingInterceptor> _logger;

    public ExceptionHandlingInterceptor(ILogger<ExceptionHandlingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (DomainException exception)
        {
            var status = new Status(StatusCode.FailedPrecondition, exception.Message);

            throw new RpcException(status);
        }
        catch (Exception exception)
        {
            _logger.LogError("Error: {ExceptionMessage}", exception.Message);

            var status = new Status(StatusCode.Internal, exception.Message);

            throw new RpcException(status);
        }
    }
}