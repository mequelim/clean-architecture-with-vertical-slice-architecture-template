using Microsoft.AspNetCore.Routing;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Abstraction
{
    public interface IApiEndpoint
    {
        void MapEndpoint(IEndpointRouteBuilder application);
    }
}