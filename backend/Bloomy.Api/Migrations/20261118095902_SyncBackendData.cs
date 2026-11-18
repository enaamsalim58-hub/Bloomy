using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bloomy.Api.Migrations
{
    /// <inheritdoc />
    public partial class SyncBackendData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "ImageUrl",
                value: "/Images/f.jpeg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "ImageUrl",
                value: "/Images/k.jpeg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "ImageUrl",
                value: "/Images/d.jpeg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "ImageUrl",
                value: "/Images/h.jpeg");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1052,
                column: "OrderDate",
                value: new DateTime(2026, 11, 18, 12, 58, 57, 484, DateTimeKind.Local).AddTicks(2952));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1053,
                column: "OrderDate",
                value: new DateTime(2026, 11, 18, 12, 58, 57, 484, DateTimeKind.Local).AddTicks(2968));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1054,
                column: "OrderDate",
                value: new DateTime(2026, 11, 18, 12, 58, 57, 484, DateTimeKind.Local).AddTicks(2977));

            migrationBuilder.InsertData(
                table: "ProductColors",
                columns: new[] { "ProductColorId", "ColorName", "HexCode", "ProductId" },
                values: new object[,]
                {
                    { 1, "lavender", "#994b94", 1 },
                    { 2, "pink", "#f472b6", 1 },
                    { 3, "cream", "#fef3c7", 1 }
                });

            migrationBuilder.InsertData(
                table: "ProductSizes",
                columns: new[] { "ProductSizeId", "ExtraPrice", "Name", "ProductId" },
                values: new object[,]
                {
                    { 1, 0m, "صغير", 1 },
                    { 2, 30m, "متوسط", 1 },
                    { 3, 60m, "كبير", 1 },
                    { 4, 0m, "متوسط", 2 }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "Description", "MainImageUrl" },
                values: new object[] { "باقة فاخرة من الورود البنفسجية والبيضاء مع تنسيق أنيق وتغليف مميز.", "/Images/f.jpeg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "Description", "MainImageUrl" },
                values: new object[] { "تنسيق أنيق من الجوري الأحمر الطبيعي.", "/Images/j.jpeg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "Description", "MainImageUrl" },
                values: new object[] { "دمية ناعمة مثالية للإهداء مع الباقات.", "/Images/d.jpeg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "Description", "MainImageUrl" },
                values: new object[] { "تشكيلة شوكولاتة بلجيكية مغلفة بعناية.", "/Images/k.jpeg" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 11, 18, 12, 58, 57, 484, DateTimeKind.Local).AddTicks(2728));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 11, 18, 12, 58, 57, 484, DateTimeKind.Local).AddTicks(2734));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductColors",
                keyColumn: "ProductColorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductColors",
                keyColumn: "ProductColorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductColors",
                keyColumn: "ProductColorId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "ProductSizeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "ProductSizeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "ProductSizeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "ProductSizeId",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/cat-roses.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/cat-choco.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/cat-dolls.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "ImageUrl",
                value: "/images/cat-gifts.jpg");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1052,
                column: "OrderDate",
                value: new DateTime(2026, 11, 14, 13, 10, 19, 771, DateTimeKind.Local).AddTicks(5342));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1053,
                column: "OrderDate",
                value: new DateTime(2026, 11, 14, 13, 10, 19, 771, DateTimeKind.Local).AddTicks(5351));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1054,
                column: "OrderDate",
                value: new DateTime(2026, 11, 14, 13, 10, 19, 771, DateTimeKind.Local).AddTicks(5355));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "Description", "MainImageUrl" },
                values: new object[] { null, "/images/lavender.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "Description", "MainImageUrl" },
                values: new object[] { null, "/images/red-roses.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "Description", "MainImageUrl" },
                values: new object[] { null, "/images/love-doll.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "Description", "MainImageUrl" },
                values: new object[] { null, "/images/choco-box.jpg" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 11, 14, 13, 10, 19, 771, DateTimeKind.Local).AddTicks(5192));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 11, 14, 13, 10, 19, 771, DateTimeKind.Local).AddTicks(5196));
        }
    }
}
