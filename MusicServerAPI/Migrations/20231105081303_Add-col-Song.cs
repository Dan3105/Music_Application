using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicServerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddcolSong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Songs",
                type: "text",
                nullable: true);
        
            migrationBuilder.DropTable(
                name: "ArtistSongs");

            migrationBuilder.DropTable(
                   name: "UserRoles");

            migrationBuilder.DropTable(
               name: "PlaylistSongs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Songs");
        }
    }
}
