namespace CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Errors.Enums
{
    public enum ErrorType
    {
        Failure,
        Unexpected,
        Validation,
        Conflict,
        NotFound,
        Unauthorized,
        Forbidden,
        Custom
    }
}