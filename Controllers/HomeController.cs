using Microsoft.AspNetCore.Mvc;
using MSIT150Site.Models;
using System.Diagnostics;

namespace MSIT150Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult First() { 
        return View();
        }

        public IActionResult GetDemo() {
            return View();
        }

        public IActionResult Register() {
            return View();
        }

        public IActionResult BootstrapCard()
        {
            return View();
        }

        public IActionResult SelectAddress()
        {
            return View();
        }

        public IActionResult Address()
        {
            return View();
        }

        public IActionResult AutoComplete()
        {
            return View();
        }

        public IActionResult _AutoCompleteSuggestions(IEnumerable<string> suggestions)
        {
            return PartialView(suggestions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}