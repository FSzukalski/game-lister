using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameLister.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddGameImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameImage_Games_GameId",
                table: "GameImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameImage",
                table: "GameImage");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "GameImage");

            migrationBuilder.RenameTable(
                name: "GameImage",
                newName: "GameImages");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "GameImages",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_GameImage_GameId",
                table: "GameImages",
                newName: "IX_GameImages_GameId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "GameImages",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "GameImages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "GameImages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameImages_Games_GameId",
                table: "GameImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameImages",
                table: "GameImages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "GameImages");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "GameImages");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "GameImages");

            migrationBuilder.RenameTable(
                name: "GameImages",
                newName: "GameImage");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "GameImage",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_GameImages_GameId",
                table: "GameImage",
                newName: "IX_GameImage_GameId");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "GameImage",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameImage",
                table: "GameImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameImage_Games_GameId",
                table: "GameImage",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
