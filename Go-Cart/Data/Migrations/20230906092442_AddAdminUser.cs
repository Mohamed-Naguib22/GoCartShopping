using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Go_Cart.Data.Migrations
{
    public partial class AddAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ImgUrl]) VALUES (N'5d4d89bf-9c66-4bbc-98d3-997356cc61c6', N'mohamednageb@gmail.com', N'MOHAMEDNAGEB@GMAIL.COM', N'mohamednageb@gmail.com', N'MOHAMEDNAGEB@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEJwh5KchnUlM6HEYgDVsz7mUt0BcwUGRPSxZKSXqoyDW1jYkYR7+6boQmNvLUZNYkQ==', N'7HNZPZEKH2PUD44CYCPWRZFYPTKEXCWJ', N'7e77cc38-3196-4ebd-8297-4dbc091d78f7', N'01069083521', 0, 0, NULL, 1, 0, N'Mohamed', N'Naguib', N'Not Set Yet')\r\n");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id = '5d4d89bf-9c66-4bbc-98d3-997356cc61c6'");
        }
    }
}
