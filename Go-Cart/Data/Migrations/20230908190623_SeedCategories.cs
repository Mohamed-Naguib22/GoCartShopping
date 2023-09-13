using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Go_Cart.Data.Migrations
{
    public partial class SeedCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "ImgUrl", "ParentCategoryId" },
                values: new object[] { "Fashion & Clothing", "\\images\\Fashion.webp", null }
            );

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "ImgUrl", "ParentCategoryId" },
                values: new object[] { "Men", "\\images\\No_Image.png", 1 }
            );

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "ImgUrl", "ParentCategoryId" },
                values: new object[] { "Women", "\\images\\No_Image.png", 1 }
            );

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "ImgUrl", "ParentCategoryId" },
                values: new object[] { "Kids", "\\images\\No_Image.png", 1 }
            );

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "ImgUrl", "ParentCategoryId" },
                values: new object[] { "Electronics & Digital", "\\images\\Electronics.webp", null }
            );

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "ImgUrl", "ParentCategoryId" },
                values: new object[] { "Phones & Tablets", "\\images\\Phones.webp", null }
            );

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "ImgUrl", "ParentCategoryId" },
                values: new object[] { "Jewelry & Watches", "\\images\\JewelryWatches.webp", null }
            );

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "ImgUrl", "ParentCategoryId" },
                values: new object[] { "Pet Supplies", "\\images\\PetSupplies.webp", null }
            );

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "ImgUrl", "ParentCategoryId" },
                values: new object[] { "Beauty & Health", "\\images\\Beauty.webp", null }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Categories]");
        }
    }
}
