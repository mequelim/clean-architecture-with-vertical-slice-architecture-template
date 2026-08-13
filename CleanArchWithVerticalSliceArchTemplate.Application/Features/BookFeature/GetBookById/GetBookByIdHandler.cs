using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction.Data;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Result;
using CleanArchWithVerticalSliceArchTemplate.Domain.Entities;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.GetBookById
{
    public sealed class GetBookByIdHandler(IRepository<Book> bookRepo) : IHandler<GetBookByIdCommand, Result<GetBookByIdResponse>>
    {
        public async Task<Result<GetBookByIdResponse>> HandleAsync(GetBookByIdCommand command, CancellationToken cancellationToken)
        {
            Book? book = await bookRepo.GetByIdAsync(command.Id, cancellationToken);

            if(book is null) return Result.Failure<GetBookByIdResponse>(BookErrors.NotFound(command.Id));

            GetBookByIdResponse response = new(book.Id, book.Title, book.Author, book.ISBN, book.Price, book.PublishedYear);

            return Result.Success(response);
        }
    }
}