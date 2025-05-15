using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace Shared.Web;

public static class EndpointDefinitionExtensions
{
    public static void RegisterEndpointDefinitions(this IEndpointRouteBuilder routeBuilder, Assembly assembly)
    {
        var routes = assembly
            .GetTypes()
            .Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>();

        foreach(var route in routes)
        {
            route.RegisterEndpoint(routeBuilder);
        }
    }
}
