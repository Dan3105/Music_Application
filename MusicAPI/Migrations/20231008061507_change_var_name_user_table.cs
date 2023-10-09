using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAPI.Migrations
{
    /// <inheritdoc />
    public partial class change_var_name_user_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Public_id",
                table: "user_account",
                newName: "public_id");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "user_account",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "user_account",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user_account",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_user_account_Email",
                table: "user_account",
                newName: "IX_user_account_email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "public_id",
                table: "user_account",
                newName: "Public_id");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "user_account",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "user_account",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "user_account",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_user_account_email",
                table: "user_account",
                newName: "IX_user_account_Email");
        }
    }
}
