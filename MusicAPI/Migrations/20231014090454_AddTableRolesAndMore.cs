using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MusicAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTableRolesAndMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_track_playlist_playlist_PlaylistId",
                table: "track_playlist");

            migrationBuilder.DropForeignKey(
                name: "FK_track_playlist_track_TrackId",
                table: "track_playlist");

            migrationBuilder.DropIndex(
                name: "IX_track_playlist_PlaylistId",
                table: "track_playlist");

            migrationBuilder.DropIndex(
                name: "IX_track_playlist_TrackId",
                table: "track_playlist");

            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "track_playlist");

            migrationBuilder.DropColumn(
                name: "TrackId",
                table: "track_playlist");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_roles_user_account_user_id",
                        column: x => x.user_id,
                        principalTable: "user_account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_track_playlist_playlist_id",
                table: "track_playlist",
                column: "playlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_role_name",
                table: "roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_track_playlist_playlist_playlist_id",
                table: "track_playlist",
                column: "playlist_id",
                principalTable: "playlist",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_track_playlist_track_track_id",
                table: "track_playlist",
                column: "track_id",
                principalTable: "track",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_track_playlist_playlist_playlist_id",
                table: "track_playlist");

            migrationBuilder.DropForeignKey(
                name: "FK_track_playlist_track_track_id",
                table: "track_playlist");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropIndex(
                name: "IX_track_playlist_playlist_id",
                table: "track_playlist");

            migrationBuilder.AddColumn<int>(
                name: "PlaylistId",
                table: "track_playlist",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrackId",
                table: "track_playlist",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_track_playlist_PlaylistId",
                table: "track_playlist",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_track_playlist_TrackId",
                table: "track_playlist",
                column: "TrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_track_playlist_playlist_PlaylistId",
                table: "track_playlist",
                column: "PlaylistId",
                principalTable: "playlist",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_track_playlist_track_TrackId",
                table: "track_playlist",
                column: "TrackId",
                principalTable: "track",
                principalColumn: "id");
        }
    }
}
