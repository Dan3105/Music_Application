using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdjustTableUserRefreshtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_refresh_token_user_account_UserId",
                table: "user_refresh_token");

            migrationBuilder.DropIndex(
                name: "IX_user_refresh_token_UserId",
                table: "user_refresh_token");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "user_refresh_token");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "user_refresh_token",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "refreshToken",
                table: "user_refresh_token",
                newName: "RefreshToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "user_refresh_token",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "user_refresh_token",
                newName: "refreshToken");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "user_refresh_token",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_refresh_token_UserId",
                table: "user_refresh_token",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_refresh_token_user_account_UserId",
                table: "user_refresh_token",
                column: "UserId",
                principalTable: "user_account",
                principalColumn: "id");
        }
    }
}
