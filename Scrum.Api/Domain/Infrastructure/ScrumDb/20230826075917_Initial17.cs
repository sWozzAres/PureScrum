using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItems_Sprints_SprintId",
                table: "ProductBacklogItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItems_Sprints_SprintId",
                table: "ProductBacklogItems",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItems_Sprints_SprintId",
                table: "ProductBacklogItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItems_Sprints_SprintId",
                table: "ProductBacklogItems",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id");
        }
    }
}
