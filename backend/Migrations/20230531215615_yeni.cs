using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class yeni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prediction_News_NewsId",
                table: "Prediction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prediction",
                table: "Prediction");

            migrationBuilder.RenameTable(
                name: "Prediction",
                newName: "Predictions");

            migrationBuilder.RenameIndex(
                name: "IX_Prediction_NewsId",
                table: "Predictions",
                newName: "IX_Predictions_NewsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Predictions",
                table: "Predictions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Sentiments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    NewsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentiments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sentiments_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sentiments_NewsId",
                table: "Sentiments",
                column: "NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_News_NewsId",
                table: "Predictions",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_News_NewsId",
                table: "Predictions");

            migrationBuilder.DropTable(
                name: "Sentiments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Predictions",
                table: "Predictions");

            migrationBuilder.RenameTable(
                name: "Predictions",
                newName: "Prediction");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_NewsId",
                table: "Prediction",
                newName: "IX_Prediction_NewsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prediction",
                table: "Prediction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prediction_News_NewsId",
                table: "Prediction",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
