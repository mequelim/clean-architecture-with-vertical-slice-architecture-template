namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.CreateBook
{
    public sealed record CreateBookResponse(
        Guid Id,
        string Title,
        string Author,
        string ISBN,
        decimal Price,
        int PublishedYear
    );
}