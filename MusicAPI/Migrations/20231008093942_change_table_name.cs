using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAPI.Migrations
{
    /// <inheritdoc />
    public partial class change_table_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_artist_ArtistId",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_Track_Album_AlbumId",
                table: "Track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Track",
                table: "Track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Album",
                table: "Album");

            migrationBuilder.RenameTable(
                name: "Track",
                newName: "track");

            migrationBuilder.RenameTable(
                name: "Album",
                newName: "album");

            migrationBuilder.RenameIndex(
                name: "IX_Track_AlbumId",
                table: "track",
                newName: "IX_track_AlbumId");

            migrationBuilder.RenameIndex(
                name: "IX_Album_ArtistId",
                table: "album",
                newName: "IX_album_ArtistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_track",
                table: "track",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_album",
                table: "album",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_album_artist_ArtistId",
                table: "album",
                column: "ArtistId",
                principalTable: "artist",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_track_album_AlbumId",
                table: "track",
                column: "AlbumId",
                principalTable: "album",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_album_artist_ArtistId",
                table: "album");

            migrationBuilder.DropForeignKey(
                name: "FK_track_album_AlbumId",
                table: "track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_track",
                table: "track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_album",
                table: "album");

            migrationBuilder.RenameTable(
                name: "track",
                newName: "Track");

            migrationBuilder.RenameTable(
                name: "album",
                newName: "Album");

            migrationBuilder.RenameIndex(
                name: "IX_track_AlbumId",
                table: "Track",
                newName: "IX_Track_AlbumId");

            migrationBuilder.RenameIndex(
                name: "IX_album_ArtistId",
                table: "Album",
                newName: "IX_Album_ArtistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Track",
                table: "Track",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Album",
                table: "Album",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_artist_ArtistId",
                table: "Album",
                column: "ArtistId",
                principalTable: "artist",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Track_Album_AlbumId",
                table: "Track",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
