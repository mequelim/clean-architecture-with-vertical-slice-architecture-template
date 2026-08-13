using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction.Data;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions.Result;
using CleanArchWithVerticalSliceArchTemplate.Domain.Entities;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.CreateBook
{
    public sealed class CreateBookHandler(IRepository<Book> callLogRepo, IUnitOfWork unitOfWork) : IHandler<CreateBookCommand, Result<CreateBookResponse>>
    {
        public async Task<Result<CreateBookResponse>> HandleAsync(CreateBookCommand command, CancellationToken cancellationToken)
        {
            Book book = new()
            {
                Id = Guid.CreateVersion7(),
                Title = command.Title,
                Author = command.Author,
                ISBN = command.ISBN,
                Price = command.Price,
                PublishedYear = command.PublishedYear
            };

            await callLogRepo.AddAsync(book, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(
                new CreateBookResponse(
                    book.Id,
                    book.Title,
                    book.Author,
                    book.ISBN,
                    book.Price,
                    book.PublishedYear
                )
            );
        }
    }
}