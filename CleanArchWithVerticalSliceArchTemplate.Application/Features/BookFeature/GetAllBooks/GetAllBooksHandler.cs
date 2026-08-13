using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction.Data;
using CleanArchWithVerticalSliceArchTemplate.Application.Data;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Result;
using CleanArchWithVerticalSliceArchTemplate.Domain.Entities;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.GetAllBooks
{
    public sealed class GetAllBooksHandler(IRepository<Book> bookRepo) : IHandler<GetAllBooksCommand, Result<GetAllBooksResponse>>
    {
        public async Task<Result<GetAllBooksResponse>> HandleAsync(GetAllBooksCommand command, CancellationToken cancellationToken)
        {
            IEnumerable<Book> books = await bookRepo.GetAllAsync(cancellationToken);
            List<BookDto> bookDto =
            [
                .. books
                    .Select((book) => new BookDto(
                        book.Id,
                        book.Title,
                        book.Author,
                        book.ISBN,
                        book.Price,
                        book.PublishedYear
                    ))
            ];

            return Result.Success(new GetAllBooksResponse(bookDto));
        }
    }
}