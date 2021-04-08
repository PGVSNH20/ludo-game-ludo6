using Microsoft.EntityFrameworkCore.Migrations;

namespace GameEngine.Migrations
{
    public partial class MovedPieceFromGamePositionToGameMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePositions_Pieces_PieceId",
                table: "GamePositions");

            migrationBuilder.DropIndex(
                name: "IX_GamePositions_PieceId",
                table: "GamePositions");

            migrationBuilder.DropColumn(
                name: "PieceId",
                table: "GamePositions");

            migrationBuilder.AddColumn<int>(
                name: "PieceId",
                table: "GameMembers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameMembers_PieceId",
                table: "GameMembers",
                column: "PieceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameMembers_Pieces_PieceId",
                table: "GameMembers",
                column: "PieceId",
                principalTable: "Pieces",
                principalColumn: "PieceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameMembers_Pieces_PieceId",
                table: "GameMembers");

            migrationBuilder.DropIndex(
                name: "IX_GameMembers_PieceId",
                table: "GameMembers");

            migrationBuilder.DropColumn(
                name: "PieceId",
                table: "GameMembers");

            migrationBuilder.AddColumn<int>(
                name: "PieceId",
                table: "GamePositions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GamePositions_PieceId",
                table: "GamePositions",
                column: "PieceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePositions_Pieces_PieceId",
                table: "GamePositions",
                column: "PieceId",
                principalTable: "Pieces",
                principalColumn: "PieceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
