using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItemDependencies_ProductBacklogItems_ChildId",
                table: "ProductBacklogItemDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItemDependencies_ProductBacklogItems_ParentId",
                table: "ProductBacklogItemDependencies");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItemDependencies_ProductBacklogItems_ChildId",
                table: "ProductBacklogItemDependencies",
                column: "ChildId",
                principalTable: "ProductBacklogItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItemDependencies_ProductBacklogItems_ParentId",
                table: "ProductBacklogItemDependencies",
                column: "ParentId",
                principalTable: "ProductBacklogItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItemDependencies_ProductBacklogItems_ChildId",
                table: "ProductBacklogItemDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItemDependencies_ProductBacklogItems_ParentId",
                table: "ProductBacklogItemDependencies");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItemDependencies_ProductBacklogItems_ChildId",
                table: "ProductBacklogItemDependencies",
                column: "ChildId",
                principalTable: "ProductBacklogItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItemDependencies_ProductBacklogItems_ParentId",
                table: "ProductBacklogItemDependencies",
                column: "ParentId",
                principalTable: "ProductBacklogItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
