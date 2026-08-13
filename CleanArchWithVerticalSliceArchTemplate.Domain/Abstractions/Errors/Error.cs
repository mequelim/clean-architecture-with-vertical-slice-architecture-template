using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Errors.Enums;

namespace CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Errors
{
    public record Error(string Code, string? Description = null, ErrorType Type = ErrorType.Failure)
    {
        public static readonly Error None = new(string.Empty);
        public static readonly Error Null = new("Error.NullValue", "The specified result value is null!");

        public static implicit operator Result.Result(Error error) => Result.Result.Failure(error);
        public static Error Failure(string code, string description) => new(code, description);
        public static Error Unexpected(string code, string description) => new(code, description, ErrorType.Unexpected);
        public static Error Validation(string code, string description) => new(code, description, ErrorType.Validation);
        public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);
        public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);
        public static Error Unauthorized(string code, string description) => new(code, description, ErrorType.Unauthorized);
        public static Error Forbidden(string code, string description) => new(code, description, ErrorType.Forbidden);
    }
}