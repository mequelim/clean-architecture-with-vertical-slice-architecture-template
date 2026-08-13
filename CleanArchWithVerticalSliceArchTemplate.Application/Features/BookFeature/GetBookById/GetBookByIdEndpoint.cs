using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Constants;
using CleanArchWithVerticalSliceArchTemplate.Application.Extensions;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.GetBookById
{
    public sealed class GetBookByIdEndpoint : IApiEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder application)
        {
            application.MapGet(
                    "books/{id:guid}",
                    async (Guid id, IHandler<GetBookByIdCommand, Result<GetBookByIdResponse>> handler, CancellationToken cancellationToken) =>
                    {
                        Result<GetBookByIdResponse> result = await handler.HandleAsync(new GetBookByIdCommand(id), cancellationToken);

                        return result.Match(
                            onSuccess: () => Results.Ok(result.Value),
                            onFailure: Results.NotFound);
                    })
                .WithTags(ApiTags.Books)
                .Produces<GetBookByIdResponse>()
                .Produces(StatusCodes.Status404NotFound);
        }
    }
}