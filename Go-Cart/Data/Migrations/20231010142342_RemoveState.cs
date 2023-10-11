using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Go_Cart.Data.Migrations
{
    public partial class RemoveState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "ShippingAddresses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "ShippingAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
