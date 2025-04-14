using Microsoft.EntityFrameworkCore;
using FashionShopMVC.Models;

namespace FashionShopMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình mối quan hệ giữa CartItem và Product
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình mối quan hệ giữa CartItem và User
            modelBuilder.Entity<CartItem>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình mối quan hệ giữa Order và User
            modelBuilder.Entity<Order>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // XÓA CÁC KHAI BÁO MỐI QUAN HỆ CỦA OrderDetail
            // Vì đã dùng [ForeignKey] trong model OrderDetail, không cần khai báo lại ở đây

            // Đảm bảo các trường không null
            modelBuilder.Entity<CartItem>()
                .Property(c => c.UserId)
                .IsRequired();

            modelBuilder.Entity<CartItem>()
                .Property(c => c.ProductId)
                .IsRequired();

            modelBuilder.Entity<CartItem>()
                .Property(c => c.Quantity)
                .IsRequired();
        }
    }
}