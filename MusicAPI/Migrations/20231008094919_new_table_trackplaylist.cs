using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAPI.Migrations
{
    /// <inheritdoc />
    public partial class new_table_trackplaylist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "track_playlist",
                columns: table => new
                {
                    track_id = table.Column<int>(type: "integer", nullable: false),
                    playlist_id = table.Column<int>(type: "integer", nullable: false),
                    Added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TrackId = table.Column<int>(type: "integer", nullable: true),
                    PlaylistId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_track_playlist", x => new { x.track_id, x.playlist_id });
                    table.ForeignKey(
                        name: "FK_track_playlist_playlist_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "playlist",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_track_playlist_track_TrackId",
                        column: x => x.TrackId,
                        principalTable: "track",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_track_playlist_PlaylistId",
                table: "track_playlist",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_track_playlist_TrackId",
                table: "track_playlist",
                column: "TrackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "track_playlist");
        }
    }
}
