using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Go_Cart.Data.Migrations
{
    public partial class AddWishlistToAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WishLists",
                columns: new[] { "ApplicationUserId", "CreatedAt" },
                values: new object[] { "5d4d89bf-9c66-4bbc-98d3-997356cc61c6", DateTime.Now }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[WishLists] WHERE Id = '5d4d89bf-9c66-4bbc-98d3-997356cc61c6'");
        }
    }
}
