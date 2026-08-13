using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Constants;
using CleanArchWithVerticalSliceArchTemplate.Application.Extensions;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.DeleteBook
{
    public class DeleteBookEndpoint : IApiEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder application)
        {
            application.MapDelete(
                    "books/{id:guid}",
                    async (Guid id, IHandler<DeleteBookCommand, Result<DeleteBookResponse>> handler, CancellationToken cancellationToken) =>
                    {
                        Result<DeleteBookResponse> result = await handler.HandleAsync(new DeleteBookCommand(id), cancellationToken);

                        return result.Match(
                            onSuccess: () => Results.Ok(result.Value),
                            onFailure: Results.NotFound
                        );
                    })
                .WithTags(ApiTags.Books)
                .Produces<DeleteBookResponse>()
                .Produces(StatusCodes.Status404NotFound);
        }
    }
}