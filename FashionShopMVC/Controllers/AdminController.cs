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
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    
                    // Tạo tên file duy nhất- làm đơn giản ko tạo tên file duy nhất
                    //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var fileName = Path.GetExtension(imageFile.FileName);

                    // Đường dẫn lưu file
                    var filePath = Path.Combine("wwwroot/images", fileName);

                    // Lưu file vào thư mục wwwroot/images
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn ảnh vào database
                    product.ImageUrl = "/images/" + fileName;
                }else
                {
                    Console.WriteLine("no file upload");
                }

                product.CreatedAt = DateTime.Now;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
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
