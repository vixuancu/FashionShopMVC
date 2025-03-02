using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FashionShopMVC.Data; // Thay thế bằng namespace chứa ApplicationDbContext
using FashionShopMVC.Models; // Thay thế bằng namespace chứa model Product
using System.Linq;
using System;

namespace FashionShopMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor để inject ApplicationDbContext
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách sản phẩm
        public IActionResult Index()
        {
            var products = _context.Products.ToList(); // Lấy tất cả sản phẩm từ database
            return View(products); // Truyền danh sách sản phẩm vào view
        }

        // Hiển thị form thêm sản phẩm
        public IActionResult Create()
        {
            return View(); // Trả về view để hiển thị form thêm sản phẩm
        }

        // Xử lý thêm sản phẩm
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid) // Kiểm tra dữ liệu hợp lệ
            {
                product.CreatedAt = DateTime.Now; // Đặt thời gian tạo
                _context.Products.Add(product); // Thêm sản phẩm vào database
                _context.SaveChanges(); // Lưu thay đổi
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách sản phẩm
            }
            return View(product); // Nếu dữ liệu không hợp lệ, trả về view với thông báo lỗi
        }

        // Xóa sản phẩm
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id); // Tìm sản phẩm theo ID
            if (product != null)
            {
                _context.Products.Remove(product); // Xóa sản phẩm
                _context.SaveChanges(); // Lưu thay đổi
            }
            return RedirectToAction("Index"); // Chuyển hướng về trang danh sách sản phẩm
        }
    }
}
