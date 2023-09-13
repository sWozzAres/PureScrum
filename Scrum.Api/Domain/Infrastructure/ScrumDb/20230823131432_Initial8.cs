using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SprintBacklogItems_ProductBacklogItemId",
                table: "SprintBacklogItems");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SprintBacklogItems",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "UQ_SprintBacklogItems_Name",
                table: "SprintBacklogItems",
                columns: new[] { "ProductBacklogItemId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_SprintBacklogItems_Name",
                table: "SprintBacklogItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SprintBacklogItems");

            migrationBuilder.CreateIndex(
                name: "IX_SprintBacklogItems_ProductBacklogItemId",
                table: "SprintBacklogItems",
                column: "ProductBacklogItemId");
        }
    }
}
