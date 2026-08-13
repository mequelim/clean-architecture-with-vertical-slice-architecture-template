using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction.Data;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Result;
using CleanArchWithVerticalSliceArchTemplate.Domain.Entities;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.DeleteBook
{
    public sealed class DeleteBookHandler(IRepository<Book> bookRepo, IUnitOfWork unitOfWork) : IHandler<DeleteBookCommand, Result<DeleteBookResponse>>
    {
        public async Task<Result<DeleteBookResponse>> HandleAsync(DeleteBookCommand command, CancellationToken cancellationToken)
        {
            Book? book = await bookRepo.GetByIdAsync(command.Id, cancellationToken);

            if(book is null) return Result.Failure<DeleteBookResponse>(BookErrors.NotFound(command.Id));

            await bookRepo.DeleteAsync(book, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(
                new DeleteBookResponse(book.Id)
            );
        }
    }
}