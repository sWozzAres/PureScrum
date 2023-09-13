using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItems_Products_ProductId",
                table: "ProductBacklogItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItems_Products_ProductId",
                table: "ProductBacklogItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItems_Products_ProductId",
                table: "ProductBacklogItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItems_Products_ProductId",
                table: "ProductBacklogItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
