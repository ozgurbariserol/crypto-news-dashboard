using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class son : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "NewsSources",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "News",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Prediction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PredictionKey = table.Column<string>(type: "text", nullable: false),
                    PredictionValue = table.Column<string>(type: "text", nullable: false),
                    NewsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prediction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prediction_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prediction_NewsId",
                table: "Prediction",
                column: "NewsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prediction");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "NewsSources",
                newName: "Name");
        }
    }
}
