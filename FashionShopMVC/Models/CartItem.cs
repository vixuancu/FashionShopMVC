using System.ComponentModel.DataAnnotations;

namespace FashionShopMVC.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Product Product { get; set; }
    }
}
