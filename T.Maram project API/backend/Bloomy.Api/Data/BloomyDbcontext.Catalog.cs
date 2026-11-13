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

                // العلاقة 1:N مع Category (سلوك الحذف Restrict بناءً على الخطة)[span_1](start_span)[span_1](end_span)
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // إعدادات جدول ProductImages (سلوك الحذف Cascade)[span_2](start_span)[span_2](end_span)
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(i => i.ProductImageId);
                entity.Property(i => i.ImageUrl).IsRequired();
                entity.HasOne(i => i.Product)
                      .WithMany(p => p.ProductImages)
                      .HasForeignKey(i => i.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // إعدادات جدول ProductSizes (سلوك الحذف Cascade)[span_3](start_span)[span_3](end_span)
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

            // إعدادات جدول ProductColors (سلوك الحذف Cascade)[span_4](start_span)[span_4](end_span)
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

            // إعدادات جدول PriceOffers (علاقة 1:1 مع Product، وسلوك الحذف Cascade)[span_5](start_span)[span_5](end_span)
            modelBuilder.Entity<PriceOffer>(entity =>
            {
                entity.HasKey(o => o.PriceOfferId);
                entity.Property(o => o.NewPrice).HasColumnType("decimal(18,2)");
                entity.HasOne(o => o.Product)
                      .WithOne(p => p.PriceOffer)
                      .HasForeignKey<PriceOffer>(o => o.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // إعدادات جدول Tags وعلاقة N:M مع Product[span_6](start_span)[span_6](end_span)
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(t => t.TagId);
                entity.Property(t => t.Name).IsRequired().HasMaxLength(50);
                entity.HasMany(t => t.Products)
                      .WithMany(p => p.Tags);
            });

            // البيانات الأولية (Seed Data) بالمعرفات الثابتة المتفق عليها في الخطة[span_7](start_span)[span_7](end_span)
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "باقات الورود", ImageUrl = "/images/cat-roses.jpg" },
                new Category { CategoryId = 2, Name = "شوكولاتة", ImageUrl = "/images/cat-choco.jpg" },
                new Category { CategoryId = 3, Name = "دمى", ImageUrl = "/images/cat-dolls.jpg" },
                new Category { CategoryId = 4, Name = "هدايا", ImageUrl = "/images/cat-gifts.jpg" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, CategoryId = 1, Name = "باقة لافندر الملكية", Price = 199m, StockQuantity = 20, IsAvailable = true, MainImageUrl = "/images/lavender.jpg" },
                new Product { ProductId = 2, CategoryId = 1, Name = "باقة ورد أحمر", Price = 149m, StockQuantity = 15, IsAvailable = true, MainImageUrl = "/images/red-roses.jpg" },
                new Product { ProductId = 3, CategoryId = 3, Name = "دمية الحب", Price = 90m, StockQuantity = 30, IsAvailable = true, MainImageUrl = "/images/love-doll.jpg" },
                new Product { ProductId = 4, CategoryId = 2, Name = "علبة شوكولاتة فاخرة", Price = 90m, StockQuantity = 25, IsAvailable = true, MainImageUrl = "/images/choco-box.jpg" }
            );
        }
    }
}