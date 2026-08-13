using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Result;
using Microsoft.Extensions.Logging;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Pipelines
{
    public sealed class LoggingDecorator<TRequest, TResponse>(ILogger<LoggingDecorator<TRequest, TResponse>> logger, IHandler<TRequest, TResponse> innerHandler)
        : IHandler<TRequest, TResponse>
    {
        public async Task<TResponse> HandleAsync(TRequest command, CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;

            logger.LogInformation($"Handling request {requestName}");

            TResponse response = await innerHandler.HandleAsync(command, cancellationToken);

            if(response is Result { IsFailure: true } result)
            {
                logger.LogWarning($"Request {requestName} failed with error code {result.Error.Code}");
            }
            else
            {
                logger.LogInformation($"Handled request {requestName}");
            }

            return response;
        }
    }
}