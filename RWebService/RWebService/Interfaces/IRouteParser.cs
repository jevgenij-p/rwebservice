using Microsoft.AspNetCore.Routing;

namespace RWebService
{
    public interface IRouteParser
    {
        Script GetScript(RouteData routeData);
    }
}
