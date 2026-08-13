using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Constants;
using CleanArchWithVerticalSliceArchTemplate.Application.Extensions;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.CreateBook
{
    public sealed class CreateBookEndpoint : IApiEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder application)
        {
            application.MapPost(
                    "books",
                    async (IHandler<CreateBookCommand, Result<CreateBookResponse>> handler, CreateBookCommand command, CancellationToken cancellationToken) =>
                    {
                        Result<CreateBookResponse> result = await handler.HandleAsync(command, cancellationToken);

                        return result.Match(
                            onSuccess: () => Results.Ok(result.Value),
                            onFailure: Results.BadRequest
                        );
                    })
                .WithTags(ApiTags.Books)
                .Produces<CreateBookResponse>()
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}