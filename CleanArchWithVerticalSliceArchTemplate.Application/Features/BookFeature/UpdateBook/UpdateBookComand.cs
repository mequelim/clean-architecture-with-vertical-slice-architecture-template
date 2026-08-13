namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.UpdateBook
{
    public sealed record UpdateBookCommand(
        Guid Id,
        string? Title,
        string? Author,
        string? ISBN,
        decimal? Price,
        int? PublishedYear
    );
}