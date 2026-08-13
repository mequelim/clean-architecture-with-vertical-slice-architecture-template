using System.Reflection;
using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Extensions;
using CleanArchWithVerticalSliceArchTemplate.Application.Pipelines;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchWithVerticalSliceArchTemplate.Application
{
    public static class DependencyInjection
    {
        extension(IServiceCollection services)
        {
            private void AddHandlersFromAssembly(Assembly assembly)
            {
                List<Type> handlerTypes =
                [
                    .. assembly.GetTypes()
                        .Where((type) => type is { IsClass: true, IsAbstract: false, ContainsGenericParameters: false })
                ];

                foreach (Type implementation in handlerTypes)
                {
                    IEnumerable<Type> handlerInterfaces = implementation
                        .GetInterfaces()
                        .Where((i) =>
                            (i.IsGenericType) &&
                            (i.GetGenericTypeDefinition() == typeof(IHandler<,>))
                        );

                    foreach (Type handlerInterface in handlerInterfaces) services.AddScoped(handlerInterface, implementation);
                }

                services.Decorate(
                    typeof(IHandler<,>),
                    typeof(ValidationDecorator<,>)
                );

                services.Decorate(
                    typeof(IHandler<,>),
                    typeof(LoggingDecorator<,>)
                );
            }

            public IServiceCollection AddApplication()
            {
                Assembly assembly = typeof(DependencyInjection).Assembly;

                services.AddValidatorsFromAssembly(assembly);
                services.AddHandlersFromAssembly(assembly);
                services.RegisterApiEndpointFromAssembly(assembly);

                return services;
            }
        }
    }
}