namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.CreateBook
{
    public sealed record CreateBookCommand(
        string Title,
        string Author,
        string ISBN,
        decimal Price,
        int PublishedYear
    );
}