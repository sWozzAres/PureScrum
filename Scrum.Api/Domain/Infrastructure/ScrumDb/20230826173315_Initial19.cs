using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItems_Products_ProductId",
                table: "ProductBacklogItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItems_Sprints_SprintId",
                table: "ProductBacklogItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintBacklogItems_ProductBacklogItems_ProductBacklogItemId",
                table: "SprintBacklogItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItems_Products_ProductId",
                table: "ProductBacklogItems",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItems_Products_ProductId1",
                table: "ProductBacklogItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SprintBacklogItems_ProductBacklogItems_ProductBacklogItemsId",
                table: "SprintBacklogItems",
                column: "ProductBacklogItemId",
                principalTable: "ProductBacklogItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItems_Products_ProductId",
                table: "ProductBacklogItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItems_Products_ProductId1",
                table: "ProductBacklogItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintBacklogItems_ProductBacklogItems_ProductBacklogItemsId",
                table: "SprintBacklogItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItems_Products_ProductId",
                table: "ProductBacklogItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItems_Sprints_SprintId",
                table: "ProductBacklogItems",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SprintBacklogItems_ProductBacklogItems_ProductBacklogItemId",
                table: "SprintBacklogItems",
                column: "ProductBacklogItemId",
                principalTable: "ProductBacklogItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
