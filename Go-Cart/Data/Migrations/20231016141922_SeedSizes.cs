using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Go_Cart.Data.Migrations
{
    public partial class SeedSizes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Name" },
                values: new object[] { "Small" }
            );

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Name" },
                values: new object[] { "Medium" }
            );

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Name" },
                values: new object[] { "Large" }
            );

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Name" },
                values: new object[] { "XL" }
            );

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Name" },
                values: new object[] { "XXL" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Sizes]");
        }
    }
}
