using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FashionShopMVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Precision(18, 2)]  // precision: tổng chữ số, scale: chữ số sau dấu phẩy
        public decimal Price { get; set; }
       // public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

