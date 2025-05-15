using Microsoft.AspNetCore.Routing;

namespace Shared.Web;

public interface IEndpointDefinition
{
    void RegisterEndpoint(IEndpointRouteBuilder endpoint);
}
