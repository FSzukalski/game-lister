using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameLister.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddListingDraftTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameImages_Games_GameId",
                table: "GameImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameImages",
                table: "GameImages");

            migrationBuilder.RenameTable(
                name: "GameImages",
                newName: "GameImage");

            migrationBuilder.RenameIndex(
                name: "IX_GameImages_GameId",
                table: "GameImage",
                newName: "IX_GameImage_GameId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price_Amount",
                table: "Games",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameImage",
                table: "GameImage",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ListingDrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Marketplace = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Subtitle = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    DescriptionHtml = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Price_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price_Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ShippingRateId = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingDrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingDrafts_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingDrafts_GameId",
                table: "ListingDrafts",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameImage_Games_GameId",
                table: "GameImage",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameImage_Games_GameId",
                table: "GameImage");

            migrationBuilder.DropTable(
                name: "ListingDrafts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameImage",
                table: "GameImage");

            migrationBuilder.RenameTable(
                name: "GameImage",
                newName: "GameImages");

            migrationBuilder.RenameIndex(
                name: "IX_GameImage_GameId",
                table: "GameImages",
                newName: "IX_GameImages_GameId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price_Amount",
                table: "Games",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameImages",
                table: "GameImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameImages_Games_GameId",
                table: "GameImages",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
