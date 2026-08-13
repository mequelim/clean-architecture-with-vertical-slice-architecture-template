using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Errors;

namespace CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Result
{
    public class Result
    {
        public bool IsSuccess { get; init; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; init; }

        // Constructor:
        protected Result(bool isSuccess, Error error)
        {
            if(((isSuccess) && (error != Error.None)) || ((!isSuccess) && (error == Error.None)))
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        // Methods:
        public static Result Success() => new(true, Error.None);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

        public static Result<TValue> Create<TValue>(TValue? value)
        {
            return (value is not null)
                ? Success(value)
                : Failure<TValue>(Error.Null);
        }
    }
}