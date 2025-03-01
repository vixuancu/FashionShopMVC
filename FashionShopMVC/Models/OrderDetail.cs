using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FashionShopMVC.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [Precision(18, 2)]  // precision: tổng chữ số, scale: chữ số sau dấu phẩy
        public decimal Price { get; set; }
    }
}
