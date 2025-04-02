using System.Diagnostics;
using FashionShopMVC.Data;
using FashionShopMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace FashionShopMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Database Context
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Take(12).ToList(); // Lấy 12 sản phẩm đầu tiên
            return View(products);
        }

        public IActionResult Giaynu()
        {
            var products = _context.Products.Where(p => p.Category == "GiayNu").ToList();
            return View(products);
        }
        public IActionResult Giaynam()
        {
            var products = _context.Products.Where(p => p.Category == "GiayNam").ToList();
            return View(products);
        }
        public IActionResult BaLo()
        {
            var products = _context.Products.Where(p => p.Category == "Balo").ToList();
            return View(products);
        }
        public IActionResult PhuKien()
        {
            var products = _context.Products.Where(p => p.Category == "PhuKien").ToList();
            return View(products);
        }
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(); // Trả về 404 nếu không có sản phẩm
            }
            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
