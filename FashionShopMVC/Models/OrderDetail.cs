using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShopMVC.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [Precision(18, 0)]  // precision: tổng chữ số, scale: chữ số sau dấu phẩy
        public decimal Price { get; set; }

        [ForeignKey("OrderId")]
        public  Order Order { get; set; }

        [ForeignKey("ProductId")]
        public  Product Product { get; set; }
    }
}
