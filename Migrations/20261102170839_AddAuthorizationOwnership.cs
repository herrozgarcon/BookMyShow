using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMyShow.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorizationOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "AppUsers",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("""
                UPDATE AppUsers
                SET NormalizedEmail = UPPER(LTRIM(RTRIM(Email))),
                    Name = CASE WHEN CHARINDEX('@', Email) > 1 THEN LEFT(Email, CHARINDEX('@', Email) - 1) ELSE 'Customer' END,
                    Role = CASE WHEN NULLIF(LTRIM(RTRIM(Role)), '') IS NULL THEN 'Customer' ELSE Role END;

                IF EXISTS (
                    SELECT NormalizedEmail
                    FROM AppUsers
                    GROUP BY NormalizedEmail
                    HAVING COUNT(*) > 1
                )
                    THROW 51000, 'Duplicate account email addresses must be resolved before applying AddAuthorizationOwnership.', 1;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppUserId",
                table: "Bookings",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_NormalizedEmail",
                table: "AppUsers",
                column: "NormalizedEmail",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AppUsers_AppUserId",
                table: "Bookings",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AppUsers_AppUserId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppUserId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_NormalizedEmail",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "AppUsers");
        }
    }
}
