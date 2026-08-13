using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction.Data;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Result;
using CleanArchWithVerticalSliceArchTemplate.Domain.Entities;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.UpdateBook
{
    public sealed class UpdateBookHandler(IRepository<Book> bookRepo, IUnitOfWork unitOfWork) : IHandler<UpdateBookCommand, Result<UpdateBookResponse>>
    {
        public async Task<Result<UpdateBookResponse>> HandleAsync(UpdateBookCommand command, CancellationToken cancellationToken)
        {
            Book? book = await bookRepo.GetByIdAsync(command.Id, cancellationToken);

            if(book is null) return Result.Failure<UpdateBookResponse>(BookErrors.NotFound(command.Id));

            book.Title = command.Title ?? book.Title;
            book.Author = command.Author ?? book.Author;
            book.ISBN = command.ISBN ?? book.ISBN;
            book.Price = command.Price ?? book.Price;
            book.PublishedYear = command.PublishedYear ?? book.PublishedYear;

            await bookRepo.UpdateAsync(book, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            UpdateBookResponse response = new(
                book.Id,
                book.Title,
                book.Author,
                book.ISBN,
                book.Price,
                book.PublishedYear
            );

            return Result.Success(response);
        }
    }
}