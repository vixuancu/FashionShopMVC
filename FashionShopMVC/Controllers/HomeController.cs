using System.Diagnostics;
using FashionShopMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace FashionShopMVC.Controllers
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

        public IActionResult Giaynu()
        {
            return View();
        }
        public IActionResult Giaynam()
        {
            return View();
        }
        public IActionResult BaLo()
        {
            return View();
        }
        public IActionResult PhuKien()
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
