using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Go_Cart.Data.Migrations
{
    public partial class AddUserGender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "security",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                schema: "security",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "security",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "security",
                table: "Users");
        }
    }
}
