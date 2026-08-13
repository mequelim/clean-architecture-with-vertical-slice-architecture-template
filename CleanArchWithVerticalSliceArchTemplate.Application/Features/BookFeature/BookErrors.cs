using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Errors;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature
{
    public static class BookErrors
    {
        public static Error NotFound(Guid id)
        {
            return Error.NotFound("Books not found", $"The Book with id \"{id}\" was not found!");
        }
    }
}