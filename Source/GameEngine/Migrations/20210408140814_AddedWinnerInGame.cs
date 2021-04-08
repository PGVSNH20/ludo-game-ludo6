using Microsoft.EntityFrameworkCore.Migrations;

namespace GameEngine.Migrations
{
    public partial class AddedWinnerInGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WinnerUserId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_WinnerUserId",
                table: "Games",
                column: "WinnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_WinnerUserId",
                table: "Games",
                column: "WinnerUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_WinnerUserId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_WinnerUserId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "WinnerUserId",
                table: "Games");
        }
    }
}
