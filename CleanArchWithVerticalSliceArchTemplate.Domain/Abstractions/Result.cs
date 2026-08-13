using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Errors;

namespace CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions
{
    public class Result<TValue> : Result.Result
    {
        // Constructor:
        protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            Value = value;
        }

        // Methods:
        public TValue Value => (IsSuccess)
            ? field!
            : throw new InvalidOperationException("The value of a failure result can not be accessed!");

        public static implicit operator Result<TValue>(TValue? value) => Create(value);

        public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);
    }
}