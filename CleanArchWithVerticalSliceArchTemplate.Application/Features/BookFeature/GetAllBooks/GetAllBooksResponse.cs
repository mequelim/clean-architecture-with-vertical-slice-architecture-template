using CleanArchWithVerticalSliceArchTemplate.Application.Data;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.GetAllBooks
{
    public sealed record GetAllBooksResponse(IEnumerable<BookDto> Books);
}