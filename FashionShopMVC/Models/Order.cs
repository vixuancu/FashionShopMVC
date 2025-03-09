using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FashionShopMVC.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Precision(18, 0)]  // precision: tổng chữ số, scale: chữ số sau dấu phẩy
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Completed, Canceled
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
