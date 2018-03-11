using Microsoft.AspNetCore.Mvc;
using RWebService;

namespace AspNetCoreService.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScriptsManager scriptsManager;

        public HomeController(IScriptsManager scriptsManager)
        {
            this.scriptsManager = scriptsManager;
        }

        public IActionResult Index()
        {
            var scrips = this.scriptsManager.GetScripts();
            return View(scrips);
        }
    }
}
