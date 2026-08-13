using FluentValidation;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.UpdateBook.Validator
{
    public class UpdateBookValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookValidator()
        {
            RuleFor((book) => book.Id)
                .NotEmpty()
                .WithMessage("Book Id is required");

            RuleFor((book) => book.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(200)
                .WithMessage("Title must not exceed 200 characters");

            RuleFor((book) => book.Author)
                .NotEmpty()
                .WithMessage("Author is required")
                .MaximumLength(100)
                .WithMessage("Author must not exceed 100 characters");

            RuleFor((book) => book.ISBN)
                .NotEmpty()
                .WithMessage("ISBN is required");

            RuleFor((book) => book.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor((book) => book.PublishedYear)
                .GreaterThan(1000)
                .WithMessage("Published year must be a valid year")
                .LessThanOrEqualTo(DateTime.UtcNow.Year)
                .WithMessage("Published year cannot be in the future");
        }
    }
}