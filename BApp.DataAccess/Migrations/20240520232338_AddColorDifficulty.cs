using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddColorDifficulty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Difficulty",
                table: "UserColors",
                newName: "ColorDifficultyId");

            migrationBuilder.CreateTable(
                name: "ColorDifficulties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ColorHexValue = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FindingCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorDifficulties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserColors_ColorDifficultyId",
                table: "UserColors",
                column: "ColorDifficultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserColors_ColorDifficulties_ColorDifficultyId",
                table: "UserColors",
                column: "ColorDifficultyId",
                principalTable: "ColorDifficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserColors_ColorDifficulties_ColorDifficultyId",
                table: "UserColors");

            migrationBuilder.DropTable(
                name: "ColorDifficulties");

            migrationBuilder.DropIndex(
                name: "IX_UserColors_ColorDifficultyId",
                table: "UserColors");

            migrationBuilder.RenameColumn(
                name: "ColorDifficultyId",
                table: "UserColors",
                newName: "Difficulty");
        }
    }
}
