using Microsoft.EntityFrameworkCore.Migrations;

namespace GameEngine.Migrations
{
    public partial class AddedPiecesDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePositions_Piece_PieceId",
                table: "GamePositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Piece",
                table: "Piece");

            migrationBuilder.RenameTable(
                name: "Piece",
                newName: "Pieces");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pieces",
                table: "Pieces",
                column: "PieceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePositions_Pieces_PieceId",
                table: "GamePositions",
                column: "PieceId",
                principalTable: "Pieces",
                principalColumn: "PieceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePositions_Pieces_PieceId",
                table: "GamePositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pieces",
                table: "Pieces");

            migrationBuilder.RenameTable(
                name: "Pieces",
                newName: "Piece");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Piece",
                table: "Piece",
                column: "PieceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePositions_Piece_PieceId",
                table: "GamePositions",
                column: "PieceId",
                principalTable: "Piece",
                principalColumn: "PieceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
