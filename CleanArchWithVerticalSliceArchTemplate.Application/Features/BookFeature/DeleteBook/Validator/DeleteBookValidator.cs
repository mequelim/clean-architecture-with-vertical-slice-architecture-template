using FluentValidation;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.DeleteBook.Validator
{
    public class DeleteBookValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookValidator()
        {
            RuleFor((book) => book.Id)
                .NotEmpty()
                .WithMessage("Book Id is required");
        }
    }
}