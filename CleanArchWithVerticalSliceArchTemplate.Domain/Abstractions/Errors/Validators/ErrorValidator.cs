using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Errors.Enums;

namespace CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Errors.Validators
{
    public record ErrorValidator(Error[] ErrorsList) : Error("Validation.General", "One or more validation errors occurred!", ErrorType.Validation)
    {
        public Error[] Errors { get; init; } = ErrorsList;

        // Method:
        public static ErrorValidator FromResults(IEnumerable<Result.Result> results)
        {
            return new ErrorValidator(
                [
                    .. results
                        .Where((result) => result.IsFailure)
                        .Select((result) => result.Error)
                ]
            );
        }
    }
}