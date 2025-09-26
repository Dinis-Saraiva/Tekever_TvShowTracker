using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvShowTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class TvshowChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "TvShows",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "TvShows",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Seasons",
                table: "TvShows",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "TvShows");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "TvShows");

            migrationBuilder.DropColumn(
                name: "Seasons",
                table: "TvShows");
        }
    }
}
