using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalsToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Goals",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goals_AppUserId",
                table: "Goals",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_AspNetUsers_AppUserId",
                table: "Goals",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_AspNetUsers_AppUserId",
                table: "Goals");

            migrationBuilder.DropIndex(
                name: "IX_Goals_AppUserId",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Goals");
        }
    }
}
