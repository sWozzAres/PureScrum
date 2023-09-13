using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial36 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductBacklogItems_ProductId",
                table: "ProductBacklogItems");

            migrationBuilder.DropIndex(
                name: "UQ_ProductBacklogItems_Name",
                table: "ProductBacklogItems");

            migrationBuilder.CreateIndex(
                name: "UQ_ProductBacklogItems_ProductId_Name",
                table: "ProductBacklogItems",
                columns: new[] { "ProductId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_ProductBacklogItems_ProductId_Name",
                table: "ProductBacklogItems");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBacklogItems_ProductId",
                table: "ProductBacklogItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "UQ_ProductBacklogItems_Name",
                table: "ProductBacklogItems",
                column: "Name",
                unique: true);
        }
    }
}
