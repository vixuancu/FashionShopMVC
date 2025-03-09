
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using FashionShopMVC.Data;
using FashionShopMVC.Models;

namespace FashionShopMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🛒 Xử lý đặt hàng
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(int ProductId, decimal Price)
        {
            // Lấy UserId từ Session (đã lưu dưới dạng string)
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) // Kiểm tra nếu chưa đăng nhập
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để đặt hàng!";
                return RedirectToAction("Login", "AuthController");
            }

            // Chuyển UserId từ string sang int
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Lỗi lấy UserId từ session!";
                return RedirectToAction("Login", "AuthController");
            }

            // Tạo đơn hàng mới
            var order = new Order
            {
                UserId = userId,
                TotalPrice = Price,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Thêm chi tiết đơn hàng
            var orderDetail = new OrderDetail
            {
                OrderId = order.Id,
                ProductId = ProductId,
                Quantity = 1,
                Price = Price
            };
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đặt hàng thành công!";
            return RedirectToAction("OrderHistory");
        }

        // 📜 Lịch sử đơn hàng của người dùng
        public async Task<IActionResult> OrderHistory()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để xem lịch sử đơn hàng!";
                return RedirectToAction("Login", "Account");
            }

            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Lỗi lấy UserId từ session!";
                return RedirectToAction("Login", "Account");
            }

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .ToListAsync();

            return View(orders);
        }
    }
}

