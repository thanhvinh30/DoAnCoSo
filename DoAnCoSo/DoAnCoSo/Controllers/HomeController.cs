using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DoAnCoSo.Controllers
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
        public IActionResult Category()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Warranty_Policy()
        {
            return View();
        }
        public IActionResult Return_Policy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
