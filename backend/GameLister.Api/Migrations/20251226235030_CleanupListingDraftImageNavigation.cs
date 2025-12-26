using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameLister.Api.Migrations
{
    /// <inheritdoc />
    public partial class CleanupListingDraftImageNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingDraftImages_GameImages_GameImageId1",
                table: "ListingDraftImages");

            migrationBuilder.DropIndex(
                name: "IX_ListingDraftImages_GameImageId1",
                table: "ListingDraftImages");

            migrationBuilder.DropColumn(
                name: "GameImageId1",
                table: "ListingDraftImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameImageId1",
                table: "ListingDraftImages",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListingDraftImages_GameImageId1",
                table: "ListingDraftImages",
                column: "GameImageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ListingDraftImages_GameImages_GameImageId1",
                table: "ListingDraftImages",
                column: "GameImageId1",
                principalTable: "GameImages",
                principalColumn: "Id");
        }
    }
}
