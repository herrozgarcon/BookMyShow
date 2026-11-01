using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMyShow.Migrations
{
    /// <inheritdoc />
    public partial class ScreensTableCreationUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Screens_Theatres_TheatreId",
                table: "Screens");

            migrationBuilder.DropIndex(
                name: "IX_Screens_TheatreId",
                table: "Screens");

            migrationBuilder.CreateIndex(
                name: "IX_Screens_TheatreId_ScreenName",
                table: "Screens",
                columns: new[] { "TheatreId", "ScreenName" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Screens_Theatres_TheatreId",
                table: "Screens",
                column: "TheatreId",
                principalTable: "Theatres",
                principalColumn: "TheatresId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Screens_Theatres_TheatreId",
                table: "Screens");

            migrationBuilder.DropIndex(
                name: "IX_Screens_TheatreId_ScreenName",
                table: "Screens");

            migrationBuilder.CreateIndex(
                name: "IX_Screens_TheatreId",
                table: "Screens",
                column: "TheatreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Screens_Theatres_TheatreId",
                table: "Screens",
                column: "TheatreId",
                principalTable: "Theatres",
                principalColumn: "TheatresId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
