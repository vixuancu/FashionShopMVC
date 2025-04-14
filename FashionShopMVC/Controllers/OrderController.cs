
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequestModel request)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để đặt hàng!" });
            }

            if (!int.TryParse(userIdStr, out int userId))
            {
                return Json(new { success = false, message = "Lỗi lấy UserId từ session!" });
            }

            var order = new Order
            {
                UserId = userId,
                TotalPrice = request.Price,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderDetail = new OrderDetail
            {
                OrderId = order.Id,
                ProductId = request.ProductId,
                Quantity = 1,
                Price = request.Price
            };
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đặt hàng thành công!" });
        }

        // Model để nhận dữ liệu từ AJAX
        public class OrderRequestModel
        {
            public int ProductId { get; set; }
            public decimal Price { get; set; }
        }


        // 📜 Lịch sử đơn hàng của người dùng
        public async Task<IActionResult> OrderHistory()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để xem lịch sử đơn hàng!";
                return RedirectToAction("Login", "Auth");
            }

            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Lỗi lấy UserId từ session!";
                return RedirectToAction("Login", "Auth");
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

