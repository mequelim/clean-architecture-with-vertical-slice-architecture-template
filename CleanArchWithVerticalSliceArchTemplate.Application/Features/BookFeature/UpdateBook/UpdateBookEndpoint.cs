using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Constants;
using CleanArchWithVerticalSliceArchTemplate.Application.Extensions;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.UpdateBook
{
    public class UpdateBookEndpoint : IApiEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder application)
        {
            application.MapPut("books/{id:guid}",
                    async (Guid id, UpdateBookCommand request, IHandler<UpdateBookCommand, Result<UpdateBookResponse>> handler, CancellationToken cancellationToken) =>
                    {
                        UpdateBookCommand updateRequest = request with { Id = id };
                        Result<UpdateBookResponse> result = await handler.HandleAsync(updateRequest, cancellationToken);

                        return result.Match(
                            onSuccess: () => Results.Ok(result.Value),
                            onFailure: Results.BadRequest
                        );
                    })
                .WithTags(ApiTags.Books)
                .Produces<UpdateBookResponse>()
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}