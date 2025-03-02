using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FashionShopMVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Giá không được để trống.")]
        [Precision(18, 2)]  // precision: tổng chữ số, scale: chữ số sau dấu phẩy
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương.")]
        public decimal Price { get; set; }
        // public int Stock { get; set; }
        [Required(ErrorMessage = "Ảnh sản phẩm không được để trống.")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Danh mục không được để trống.")]
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

