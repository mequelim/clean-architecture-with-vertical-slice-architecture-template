namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.GetBookById
{
    public sealed record GetBookByIdResponse(
        Guid Id,
        string Title,
        string Author,
        string ISBN,
        decimal Price,
        int PublishedYear
    );
}