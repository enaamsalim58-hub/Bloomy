using Bloomy.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Data
{
    public partial class BloomyDbContext
    {
        // تعريف كافة جداول الكتالوج لتظهر في قاعدة البيانات
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<PriceOffer> PriceOffers { get; set; }
        public DbSet<Tag> Tags { get; set; }

        partial void ConfigureCatalog(ModelBuilder modelBuilder)
        {
            // إعدادات جدول Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.CategoryId);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            // إعدادات جدول Product والعلاقة مع القسم (Category)
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(150);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");

                // العلاقة 1:N مع Category (سلوك الحذف Restrict بناءً على الخطة)
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // إعدادات جدول ProductImages (سلوك الحذف Cascade)
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(i => i.ProductImageId);
                entity.Property(i => i.ImageUrl).IsRequired();
                entity.HasOne(i => i.Product)
                      .WithMany(p => p.ProductImages)
                      .HasForeignKey(i => i.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // إعدادات جدول ProductSizes (سلوك الحذف Cascade)
            modelBuilder.Entity<ProductSize>(entity =>
            {
                entity.HasKey(s => s.ProductSizeId);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(50);
                entity.Property(s => s.ExtraPrice).HasColumnType("decimal(18,2)");
                entity.HasOne(s => s.Product)
                      .WithMany(p => p.ProductSizes)
                      .HasForeignKey(s => s.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // إعدادات جدول ProductColors (سلوك الحذف Cascade)
            modelBuilder.Entity<ProductColor>(entity =>
            {
                entity.HasKey(c => c.ProductColorId);
                entity.Property(c => c.ColorName).IsRequired().HasMaxLength(50);
                entity.Property(c => c.HexCode).IsRequired().HasMaxLength(20);
                entity.HasOne(c => c.Product)
                      .WithMany(p => p.ProductColors)
                      .HasForeignKey(c => c.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // إعدادات جدول PriceOffers (علاقة 1:1 مع Product، وسلوك الحذف Cascade)
            modelBuilder.Entity<PriceOffer>(entity =>
            {
                entity.HasKey(o => o.PriceOfferId);
                entity.Property(o => o.NewPrice).HasColumnType("decimal(18,2)");
                entity.HasOne(o => o.Product)
                      .WithOne(p => p.PriceOffer)
                      .HasForeignKey<PriceOffer>(o => o.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // إعدادات جدول Tags وعلاقة N:M مع Product
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(t => t.TagId);
                entity.Property(t => t.Name).IsRequired().HasMaxLength(50);
                entity.HasMany(t => t.Products)
                      .WithMany(p => p.Tags);
            });

            // البيانات الأولية (Seed Data) بالمعرفات الثابتة ومسارات الصور المعتمدة
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "باقات الورود", ImageUrl = "/Images/f.jpeg" },
                new Category { CategoryId = 2, Name = "شوكولاتة", ImageUrl = "/Images/k.jpeg" },
                new Category { CategoryId = 3, Name = "دمى", ImageUrl = "/Images/d.jpeg" },
                new Category { CategoryId = 4, Name = "هدايا", ImageUrl = "/Images/h.jpeg" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    CategoryId = 1,
                    Name = "باقة لافندر الملكية",
                    Description = "باقة فاخرة من الورود البنفسجية والبيضاء مع تنسيق أنيق وتغليف مميز.",
                    Price = 199m,
                    StockQuantity = 20,
                    IsAvailable = true,
                    MainImageUrl = "/Images/f.jpeg"
                },
                new Product
                {
                    ProductId = 2,
                    CategoryId = 1,
                    Name = "باقة ورد أحمر",
                    Description = "تنسيق أنيق من الجوري الأحمر الطبيعي.",
                    Price = 149m,
                    StockQuantity = 15,
                    IsAvailable = true,
                    MainImageUrl = "/Images/j.jpeg"
                },
                new Product
                {
                    ProductId = 3,
                    CategoryId = 3,
                    Name = "دمية الحب",
                    Description = "دمية ناعمة مثالية للإهداء مع الباقات.",
                    Price = 90m,
                    StockQuantity = 30,
                    IsAvailable = true,
                    MainImageUrl = "/Images/d.jpeg"
                },
                new Product
                {
                    ProductId = 4,
                    CategoryId = 2,
                    Name = "علبة شوكولاتة فاخرة",
                    Description = "تشكيلة شوكولاتة بلجيكية مغلفة بعناية.",
                    Price = 90m,
                    StockQuantity = 25,
                    IsAvailable = true,
                    MainImageUrl = "/Images/k.jpeg"
                }
            );

            modelBuilder.Entity<ProductSize>().HasData(
                new ProductSize { ProductSizeId = 1, ProductId = 1, Name = "صغير", ExtraPrice = 0m },
                new ProductSize { ProductSizeId = 2, ProductId = 1, Name = "متوسط", ExtraPrice = 30m },
                new ProductSize { ProductSizeId = 3, ProductId = 1, Name = "كبير", ExtraPrice = 60m },
                new ProductSize { ProductSizeId = 4, ProductId = 2, Name = "متوسط", ExtraPrice = 0m }
            );

            modelBuilder.Entity<ProductColor>().HasData(
                new ProductColor { ProductColorId = 1, ProductId = 1, ColorName = "lavender", HexCode = "#994b94" },
                new ProductColor { ProductColorId = 2, ProductId = 1, ColorName = "pink", HexCode = "#f472b6" },
                new ProductColor { ProductColorId = 3, ProductId = 1, ColorName = "cream", HexCode = "#fef3c7" }
            );
        }
    }
}