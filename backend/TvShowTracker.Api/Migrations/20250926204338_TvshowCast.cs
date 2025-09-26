using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvShowTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class TvshowCast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actors_TvShows_TvShowId",
                table: "Actors");

            migrationBuilder.DropIndex(
                name: "IX_Actors_TvShowId",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "TvShowId",
                table: "Actors");

            migrationBuilder.CreateTable(
                name: "ActorTvShow",
                columns: table => new
                {
                    CastId = table.Column<int>(type: "INTEGER", nullable: false),
                    TvShowsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorTvShow", x => new { x.CastId, x.TvShowsId });
                    table.ForeignKey(
                        name: "FK_ActorTvShow_Actors_CastId",
                        column: x => x.CastId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorTvShow_TvShows_TvShowsId",
                        column: x => x.TvShowsId,
                        principalTable: "TvShows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorTvShow_TvShowsId",
                table: "ActorTvShow",
                column: "TvShowsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorTvShow");

            migrationBuilder.AddColumn<int>(
                name: "TvShowId",
                table: "Actors",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actors_TvShowId",
                table: "Actors",
                column: "TvShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actors_TvShows_TvShowId",
                table: "Actors",
                column: "TvShowId",
                principalTable: "TvShows",
                principalColumn: "Id");
        }
    }
}
