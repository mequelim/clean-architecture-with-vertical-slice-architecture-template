using System.Reflection;
using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArchWithVerticalSliceArchTemplate.Application.Extensions
{
    public static class MapEndpointExtensions
    {
        public static IServiceCollection RegisterApiEndpointFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            Type[] endpointTypes = assembly.GetTypes();
            ServiceDescriptor[] servicesDescriptors = [
                .. endpointTypes
                    .Select((type) => ServiceDescriptor.Transient(typeof(IApiEndpoint), type))
            ];

            services.TryAddEnumerable(servicesDescriptors);

            return services;
        }

        public static WebApplication MapApiEndpoints(this WebApplication application)
        {
            IEnumerable<IApiEndpoint> endpoints = application.Services.GetRequiredService<IEnumerable<IApiEndpoint>>();

            foreach(IApiEndpoint endpoint in endpoints) endpoint.MapEndpoint(application);

            return application;
        }
    }
}