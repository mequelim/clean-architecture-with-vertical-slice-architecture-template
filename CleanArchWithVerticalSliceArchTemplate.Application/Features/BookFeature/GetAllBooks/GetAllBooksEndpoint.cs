using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Constants;
using CleanArchWithVerticalSliceArchTemplate.Application.Extensions;
using CleanArchWithVerticalSliceArchTemplate.Domain.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Features.BookFeature.GetAllBooks
{
    public class GetAllBooksEndpoint : IApiEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder application)
        {
            application.MapGet(
                    "books",
                    async (IHandler<GetAllBooksCommand, Result<GetAllBooksResponse>> handler, CancellationToken cancellationToken) =>
                    {
                        Result<GetAllBooksResponse> result = await handler.HandleAsync(new GetAllBooksCommand(), cancellationToken);

                        return result.Match(
                            onSuccess: () => Results.Ok(result.Value),
                            onFailure: Results.BadRequest
                        );
                    })
                .WithTags(ApiTags.Books)
                .Produces<GetAllBooksResponse>()
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}