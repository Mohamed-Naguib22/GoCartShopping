using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Go_Cart.Data.Migrations
{
    public partial class SeedColors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Name", "Value" },
                values: new object[] { "Red", "danger" }
            );

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Name", "Value" },
                values: new object[] { "Green", "success" }
            );

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Name", "Value" },
                values: new object[] { "Yellow", "warning" }
            );

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Name" , "Value" },
                values: new object[] { "Blue", "primary" }
            );

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Name", "Value" },
                values: new object[] { "Black", "dark" }
            );

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Name", "Value" },
                values: new object[] { "Gray", "secondary" }
            );

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Name", "Value" },
                values: new object[] { "Teal", "info" }
            );

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Name", "Value" },
                values: new object[] { "White", "white" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Colors]");
        }
    }
}
