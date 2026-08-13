using FluentValidation;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.GetBookById.Validator
{
    public class GetBookByIdValidator : AbstractValidator<GetBookByIdCommand>
    {
        public GetBookByIdValidator()
        {
            RuleFor((book) => book.Id)
                .NotEmpty()
                .WithMessage("Book id is required");
        }
    }
}