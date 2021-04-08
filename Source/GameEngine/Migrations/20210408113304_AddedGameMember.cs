using Microsoft.EntityFrameworkCore.Migrations;

namespace GameEngine.Migrations
{
    public partial class AddedGameMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Games_GameId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_GameId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "GameMembers",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameMembers", x => new { x.GameId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GameMembers_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameMembers_UserId",
                table: "GameMembers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameMembers");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GameId",
                table: "Users",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Games_GameId",
                table: "Users",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
