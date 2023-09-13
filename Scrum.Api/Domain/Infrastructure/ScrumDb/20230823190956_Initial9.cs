using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItems_ProductBacklogItems_ProductBacklogItemId",
                table: "ProductBacklogItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductBacklogItems_ProductBacklogItemId",
                table: "ProductBacklogItems");

            migrationBuilder.DropColumn(
                name: "ProductBacklogItemId",
                table: "ProductBacklogItems");

            migrationBuilder.CreateTable(
                name: "ProductBacklogItemProductBacklogItem",
                columns: table => new
                {
                    ChildrenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBacklogItemProductBacklogItem", x => new { x.ChildrenId, x.ParentsId });
                    table.ForeignKey(
                        name: "FK_ProductBacklogItemProductBacklogItem_ProductBacklogItems_ChildrenId",
                        column: x => x.ChildrenId,
                        principalTable: "ProductBacklogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductBacklogItemProductBacklogItem_ProductBacklogItems_ParentsId",
                        column: x => x.ParentsId,
                        principalTable: "ProductBacklogItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBacklogItemProductBacklogItem_ParentsId",
                table: "ProductBacklogItemProductBacklogItem",
                column: "ParentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBacklogItemProductBacklogItem");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductBacklogItemId",
                table: "ProductBacklogItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBacklogItems_ProductBacklogItemId",
                table: "ProductBacklogItems",
                column: "ProductBacklogItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItems_ProductBacklogItems_ProductBacklogItemId",
                table: "ProductBacklogItems",
                column: "ProductBacklogItemId",
                principalTable: "ProductBacklogItems",
                principalColumn: "Id");
        }
    }
}
