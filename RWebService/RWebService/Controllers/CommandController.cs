using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RWebService;
using RWebService.Helpers;
using RWebService.Logic;
using System.IO;

namespace AspNetCoreService.Controllers
{
    public class CommandController : Controller
    {
        private readonly IInterpretersManager interpretersManager;
        private readonly IRouteParser routeParser;
        private readonly ILogger logger;

        public CommandController(ILogger<CommandController> logger, IRouteParser routeParser, IInterpretersManager interpretersManager)
        {
            this.logger = logger;
            this.interpretersManager = interpretersManager;
            this.routeParser = routeParser;
        }

        [CleanupFilter]
        public ActionResult Index()
        {
            dynamic result = null;
            ViewBag.TempDirectory = null;

            var script = routeParser.GetScript(RouteData);
            if (script == null)
            {
                return BadRequest();
            }

            var interpreter = interpretersManager.GetInterpreter(script.Interpreter);
            if (interpreter == null)
            {
                return BadRequest();
            }

            if (Request.Method == "POST")
            {
                // We will save posted data into a temp directory
                script.CopyToTemp = true;
            }

            if (script.CopyToTemp)
            {
                ViewBag.TempDirectory = Utilities.CreateTempDirectory(interpretersManager.WorkingDirectory);
                Utilities.CopyFiles(script.Location, ViewBag.TempDirectory);
            }

            if (Request.Method == "POST")
            {
                string path = Path.Combine(ViewBag.TempDirectory, "input.json");
                Utilities.SavePostRequestData(this.Request, path);
            }

            string arguments = ArgumentsBuilder.Build(interpreter, script, this.Request);
            string workDirectory = script.CopyToTemp ? ViewBag.TempDirectory : script.Location;

            var process = ProcessExt.Create();
            process.NoOutput = script.NoOutput;
            process.StartInfo.FileName = interpreter.Path;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WorkingDirectory = workDirectory;

            process.StartAndWait();

            var outputFile = new OutputFile(script);

            if (outputFile.IsDefined)
            {
                string path = Path.Combine(workDirectory, outputFile.FileName);
                logger.LogInformation($"Output file: \"{path}\"");

                if (!System.IO.File.Exists(path))
                {
                    logger.LogError($"Output file \"{outputFile.FileName}\" not found");
                    return BadRequest();
                }

                if (outputFile.IsJson)
                {
                    result = outputFile.ReadJson(path);
                }
                else
                {
                    return outputFile.ReadFileStream(path);
                }
            }

            return Json(new { Output = process.Output, Result = result },
                        new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}
