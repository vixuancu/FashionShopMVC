using Microsoft.AspNetCore.Http; //  thư viện này để sử dụng Session
using FashionShopMVC.Data;

using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using FashionShopMVC.Models;

namespace FashionShopMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string PasswordHash)
        {
           // Console.WriteLine($"Password nhận từ form: {password}");
            if (string.IsNullOrEmpty(PasswordHash))
            {
                ViewBag.Error = "Mật khẩu không được để trống!";
                return View();
            }
            string hashedPassword = HashPassword(PasswordHash);
            var user = _context.Users.FirstOrDefault(u => u.Email == Email && u.PasswordHash == hashedPassword);

            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("UserRole", user.Role);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng!";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user, string ConfirmPassword)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ViewBag.Error = "Email này đã được đăng ký!";
                return View();
            }
            if (string.IsNullOrEmpty(user.FullName))
            {
                ViewBag.Error = "Họ và tên không được để trống.";
                return View();
            }
            if (string.IsNullOrEmpty(user.Email) || !Regex.IsMatch(user.Email, @"\S+@\S+\.\S+"))
            {
                ViewBag.Error = "Email không hợp lệ.";
                return View();
            }
            if (string.IsNullOrEmpty(user.PasswordHash) || user.PasswordHash.Length < 6)
            {
                ViewBag.Error = "Mật khẩu phải có ít nhất 6 ký tự.";
                return View();
            }
            // Kiểm tra mật khẩu nhập lại có khớp không
            if (user.PasswordHash != ConfirmPassword)
            {
                ViewBag.Error = "Mật khẩu nhập lại không khớp!";
                return View();
            }
            // mã hoá mật khẩu 
            user.PasswordHash = HashPassword(user.PasswordHash);
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

