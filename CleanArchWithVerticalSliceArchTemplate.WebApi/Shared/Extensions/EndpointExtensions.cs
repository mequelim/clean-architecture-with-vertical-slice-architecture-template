using System.Reflection;
using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;

namespace CleanArchWithVerticalSliceArchTemplate.WebApi.Shared.Extensions
{
    public static class EndpointExtensions
    {
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder application, Assembly assembly)
        {
            IEnumerable<Type> endpointTypes = assembly
                .GetTypes()
                .Where((type) => typeof(IApiEndpoint).IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

            foreach(Type type in endpointTypes)
            {
                if(Activator.CreateInstance(type) is IApiEndpoint endpoint) endpoint.MapEndpoint(application);
            }

            return application;
        }
    }
}