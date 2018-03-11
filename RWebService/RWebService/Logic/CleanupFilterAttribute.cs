using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;

namespace RWebService.Logic
{
    public class CleanupFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var controller = context.Controller as AspNetCoreService.Controllers.CommandController;
            var tempDirectory = controller.ViewBag.TempDirectory as string;

            if (!string.IsNullOrEmpty(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }

            base.OnResultExecuted(context);
        }
    }
}
