using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvShowTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class TvshowUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TvShows_AspNetUsers_ApplicationUserId",
                table: "TvShows");

            migrationBuilder.DropIndex(
                name: "IX_TvShows_ApplicationUserId",
                table: "TvShows");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TvShows");

            migrationBuilder.CreateTable(
                name: "UserFavoriteTvShows",
                columns: table => new
                {
                    FavoriteTvShowsId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersWhoFavouritedId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteTvShows", x => new { x.FavoriteTvShowsId, x.UsersWhoFavouritedId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteTvShows_AspNetUsers_UsersWhoFavouritedId",
                        column: x => x.UsersWhoFavouritedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteTvShows_TvShows_FavoriteTvShowsId",
                        column: x => x.FavoriteTvShowsId,
                        principalTable: "TvShows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteTvShows_UsersWhoFavouritedId",
                table: "UserFavoriteTvShows",
                column: "UsersWhoFavouritedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteTvShows");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TvShows",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TvShows_ApplicationUserId",
                table: "TvShows",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TvShows_AspNetUsers_ApplicationUserId",
                table: "TvShows",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
