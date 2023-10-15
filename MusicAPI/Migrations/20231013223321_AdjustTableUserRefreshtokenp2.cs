using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdjustTableUserRefreshtokenp2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "user_refresh_token",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_user_refresh_token_UserId",
                table: "user_refresh_token",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_refresh_token_user_account_UserId",
                table: "user_refresh_token",
                column: "UserId",
                principalTable: "user_account",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
