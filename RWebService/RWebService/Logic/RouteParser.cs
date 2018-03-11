using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;

namespace RWebService.Logic
{
    public class RouteParser : IRouteParser
    {
        private readonly ILogger logger;

        private ScriptSettings scriptSettings { get; set; }

        public RouteParser(ILogger<RouteParser> logger, IOptions<ScriptSettings> scriptSettings)
        {
            this.logger = logger;
            this.scriptSettings = scriptSettings.Value;
        }

        public Script GetScript(RouteData routeData)
        {
            var routeDataValues = routeData.Values["catchall"] as string;
            if (routeDataValues != null)
            {
                var route = routeDataValues?.Split('/').Where(x => x != string.Empty).ToList().FirstOrDefault();
                if (route != null)
                {
                    logger.LogInformation($"Route: {route}");
                    var scripts = scriptSettings.Scripts?.Where(x => string.Compare(x.Route, route, ignoreCase: true) == 0);

                    var script = scripts?.FirstOrDefault();
                    if (script == null)
                    {
                        logger.LogError($"Route \"{route}\" not found");
                        return null;
                    }

                    logger.LogInformation($"Script: {script.Name}. Interpreter: {script.Interpreter}");

                    return script;
                }
            }

            return null;
        }
    }
}
