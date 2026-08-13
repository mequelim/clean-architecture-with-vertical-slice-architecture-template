using System.Reflection;
using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Errors;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Errors.Validators;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Result;
using FluentValidation;
using FluentValidation.Results;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Pipelines
{
    public class ValidationDecorator<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, IHandler<TRequest, TResponse> innerHandler)
        : IHandler<TRequest, TResponse>
    {
        private static TResponse CreateFailureResponse(ValidationFailure[] failures)
        {
            if(typeof(TResponse) == typeof(Result)) return (TResponse)(object)Result.Failure(CreateValidationError(failures));
            if((!typeof(TResponse).IsGenericType) || (typeof(TResponse).GetGenericTypeDefinition() != typeof(Result)))
            {
                throw new InvalidOperationException(
                    $"ValidationDecorator supports only {nameof(Result)} and {nameof(Result)} responses. Received {typeof(TResponse).FullName}."
                );
            }

            Type valueType = typeof(TResponse).GetGenericArguments()[0];
            MethodInfo failureMethod = typeof(Result)
                .GetMethods()
                .First((methodInfo) => (methodInfo is { Name: nameof(Result.Failure), IsGenericMethodDefinition: true }) &&
                                       (methodInfo.GetParameters().Length.Equals(1))
                );

            object? typedFailure = failureMethod
                .MakeGenericMethod(valueType)
                .Invoke(null, [CreateValidationError(failures)]);

            return (TResponse)typedFailure!;

        }

        private static ErrorValidator CreateValidationError(ValidationFailure[] validationFailures)
        {
            return new ErrorValidator(
                [
                    .. validationFailures
                        .Select((failure) => Error.Validation(failure.ErrorCode, failure.ErrorMessage))
                ]
            );
        }

        public async Task<TResponse> HandleAsync(TRequest command, CancellationToken cancellationToken)
        {
            if(!validators.Any()) return await innerHandler.HandleAsync(command, cancellationToken);

            ValidationContext<TRequest> context = new(command);
            ValidationFailure[] failures =
            [
                .. (
                    await Task.WhenAll(
                        validators.Select(v => v.ValidateAsync(context, cancellationToken))
                    )
                )
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
            ];

            if(failures.Length.Equals(0)) return await innerHandler.HandleAsync(command, cancellationToken);

            return CreateFailureResponse(failures);
        }
    }
}