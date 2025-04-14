using FashionShopMVC.Data;
using FashionShopMVC.Helpers;
using FashionShopMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FashionShopMVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Console.WriteLine($"AddToCart - UserIdClaim: {userIdClaim}");
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để thêm vào giỏ hàng!" });
                }

                var userExists = _context.Users.Any(u => u.Id == userId);
                if (!userExists)
                {
                    return Json(new { success = false, message = "Người dùng không tồn tại!" });
                }

                var product = _context.Products.Find(productId);
                Console.WriteLine($"Product found: {product != null}, ProductId: {productId}");
                if (product == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
                }

                var cartItem = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
                Console.WriteLine($"CartItem found: {cartItem != null}");

                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cartItem = new CartItem
                    {
                        UserId = userId,
                        ProductId = productId,
                        Quantity = 1,
                        CreatedAt = DateTime.Now
                    };
                    _context.CartItems.Add(cartItem);
                }

                _context.SaveChanges();
                Console.WriteLine("CartItem saved successfully");

                var cartCount = _context.CartItems.Where(c => c.UserId == userId).Sum(c => c.Quantity);

                return Json(new
                {
                    success = true,
                    message = "Đã thêm vào giỏ hàng!",
                    cartCount = cartCount
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddToCart: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi, vui lòng thử lại!" });
            }
        }
        // Lấy danh sách sản phẩm trong giỏ
        [HttpGet]
        public IActionResult GetCartItems()
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Console.WriteLine($"GetCartItems - UserIdClaim: {userIdClaim}");
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Json(new { success = false, items = new List<object>(), message = "Vui lòng đăng nhập để xem giỏ hàng!" });
                }

                var cartItems = _context.CartItems
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Product)
                    .Select(c => new
                    {
                        c.Id,
                        c.UserId,
                        c.ProductId,
                        c.Quantity,
                        ProductName = c.Product != null ? c.Product.Name : "Sản phẩm không tồn tại",
                        Price = c.Product != null ? c.Product.Price : 0
                    })
                    .ToList();

                return Json(new { success = true, items = cartItems });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCartItems: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi tải giỏ hàng!" });
            }
        }


    }
}
