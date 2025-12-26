using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameLister.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddListingDraftImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingDraftImages_GameImages_GameImageId",
                table: "ListingDraftImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingDraftImages",
                table: "ListingDraftImages");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ListingDraftImages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ListingDraftImages",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "GameImageId1",
                table: "ListingDraftImages",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingDraftImages",
                table: "ListingDraftImages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ListingDraftImages_GameImageId1",
                table: "ListingDraftImages",
                column: "GameImageId1");

            migrationBuilder.CreateIndex(
                name: "IX_ListingDraftImages_ListingDraftId_GameImageId",
                table: "ListingDraftImages",
                columns: new[] { "ListingDraftId", "GameImageId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingDraftImages_GameImages_GameImageId",
                table: "ListingDraftImages",
                column: "GameImageId",
                principalTable: "GameImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingDraftImages_GameImages_GameImageId1",
                table: "ListingDraftImages",
                column: "GameImageId1",
                principalTable: "GameImages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingDraftImages_GameImages_GameImageId",
                table: "ListingDraftImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingDraftImages_GameImages_GameImageId1",
                table: "ListingDraftImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingDraftImages",
                table: "ListingDraftImages");

            migrationBuilder.DropIndex(
                name: "IX_ListingDraftImages_GameImageId1",
                table: "ListingDraftImages");

            migrationBuilder.DropIndex(
                name: "IX_ListingDraftImages_ListingDraftId_GameImageId",
                table: "ListingDraftImages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ListingDraftImages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ListingDraftImages");

            migrationBuilder.DropColumn(
                name: "GameImageId1",
                table: "ListingDraftImages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingDraftImages",
                table: "ListingDraftImages",
                columns: new[] { "ListingDraftId", "GameImageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ListingDraftImages_GameImages_GameImageId",
                table: "ListingDraftImages",
                column: "GameImageId",
                principalTable: "GameImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
