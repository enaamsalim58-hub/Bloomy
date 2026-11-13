using Microsoft.EntityFrameworkCore;
using Bloomy.Api.Models;

namespace Bloomy.Api.Data
{
    public partial class BloomyDbContext
    {
        // 1.  ⁄—Ì› «·Ãœ«Ê· (DbSets) «·Œ«’… »«·„” Œœ„Ì‰ Ê«·„»Ì⁄« 
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Favorite> Favorites { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        // 2. ≈⁄œ«œ«  «·⁄·«ﬁ«  ÊﬁÊ«⁄œ «·Õ–› Ê«·»Ì«‰«  «·√Ê·Ì… (Seed)
        public void ConfigureSales(ModelBuilder modelBuilder)
        {
            // «·„› «Õ «·„—ﬂ» ·ÃœÊ· «·„›÷·… (Favorite) »Ì‰ «·„” Œœ„ Ê«·„‰ Ã
            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.UserId, f.ProductId });

            // ”·Êﬂ «·Õ–›: ⁄·«ﬁ… «·ÿ·» Ê⁄‰«’— «·ÿ·» Cascade[cite: 4]
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ”·Êﬂ «·Õ–›: „‰⁄ Õ–› «·„” Œœ„ ≈–« ﬂ«‰ ·Â ÿ·»«  «— »«ÿÌ… Restrict[cite: 4]
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ⁄·«ﬁ… «·„” Œœ„ »«·⁄‰«ÊÌ‰ Cascade
            modelBuilder.Entity<User>()
                .HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3. «·»Ì«‰«  «·√Ê·Ì… (Seed Data) Õ”» «·Œÿ…[cite: 4]
            // „” Œœ„ Admin Ê„” Œœ„… ”«—…[cite: 4]
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, FullName = "Admin User", Email = "admin@bloomy.com", PasswordHash = "123456", Role = "Admin", CreatedAt = DateTime.Now },
                new User { UserId = 2, FullName = "”«—…", Email = "sara@bloomy.com", PasswordHash = "123456", Role = "Customer", CreatedAt = DateTime.Now }
            );

            // ⁄‰Ê«‰  Ã—Ì»Ì ·”«—…
            modelBuilder.Entity<Address>().HasData(
                new Address { AddressId = 1, UserId = 2, Title = "«·„‰“·", AddressLine = "«·„ﬂ·« - »‰ ⁄“Ê‰" }
            );

            // «·ÿ·»«  «· Ã—Ì»Ì… »—ﬁ„ 1052 Ê 1053 Ê 1054[cite: 4]
            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1052, UserId = 2, OrderDate = DateTime.Now, Status = OrderStatus.New, Subtotal = 199, DeliveryFee = 10, Discount = 0, Total = 209, ReceiverName = "”«—…", ReceiverPhone = "770000000", DeliveryAddress = "«·„ﬂ·«" },
                new Order { OrderId = 1053, UserId = 2, OrderDate = DateTime.Now, Status = OrderStatus.Processing, Subtotal = 149, DeliveryFee = 10, Discount = 0, Total = 159, ReceiverName = "”«—…", ReceiverPhone = "770000000", DeliveryAddress = "«·„ﬂ·«" },
                new Order { OrderId = 1054, UserId = 2, OrderDate = DateTime.Now, Status = OrderStatus.Delivered, Subtotal = 180, DeliveryFee = 10, Discount = 0, Total = 190, ReceiverName = "”«—…", ReceiverPhone = "770000000", DeliveryAddress = "«·„ﬂ·«" }
            );

            // ⁄‰«’— «·ÿ·»«  „— »ÿ… »„‰ Ã«  «·ﬂ «·ÊÃ »«·„⁄—›«  «·À«» … (1, 2, 3, 4)[cite: 4]
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { OrderItemId = 1, OrderId = 1052, ProductId = 1, Quantity = 1, UnitPrice = 199 },
                new OrderItem { OrderItemId = 2, OrderId = 1053, ProductId = 2, Quantity = 1, UnitPrice = 149 },
                new OrderItem { OrderItemId = 3, OrderId = 1054, ProductId = 3, Quantity = 1, UnitPrice = 90 },
                new OrderItem { OrderItemId = 4, OrderId = 1054, ProductId = 4, Quantity = 1, UnitPrice = 90 }
            );
        }
    }
}