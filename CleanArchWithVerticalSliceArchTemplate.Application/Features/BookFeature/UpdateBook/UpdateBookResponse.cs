namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.UpdateBook
{
    public sealed record UpdateBookResponse(
        Guid Id,
        string? Title,
        string? Author,
        string? ISBN,
        decimal? Price,
        int? PublishedYear
   );
}