using Microsoft.EntityFrameworkCore.Migrations;

namespace GameEngine.Migrations
{
    public partial class AddedPiece : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "GamePositions");

            migrationBuilder.AddColumn<int>(
                name: "PieceId",
                table: "GamePositions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Piece",
                columns: table => new
                {
                    PieceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piece", x => x.PieceId);
                });

            migrationBuilder.InsertData(
                table: "Piece",
                columns: new[] { "PieceId", "Color" },
                values: new object[,]
                {
                    { 1, "Blue" },
                    { 2, "Green" },
                    { 3, "Red" },
                    { 4, "Yellow" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePositions_PieceId",
                table: "GamePositions",
                column: "PieceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePositions_Piece_PieceId",
                table: "GamePositions",
                column: "PieceId",
                principalTable: "Piece",
                principalColumn: "PieceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePositions_Piece_PieceId",
                table: "GamePositions");

            migrationBuilder.DropTable(
                name: "Piece");

            migrationBuilder.DropIndex(
                name: "IX_GamePositions_PieceId",
                table: "GamePositions");

            migrationBuilder.DropColumn(
                name: "PieceId",
                table: "GamePositions");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "GamePositions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
